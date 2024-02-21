using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.DataAccess.StaticData;
using BlackJackHusofication.Model.Exceptions;
using BlackJackHusofication.Model.Models;
using Microsoft.AspNetCore.SignalR;
using System.Numerics;

namespace BlackJackHusofication.Business.SignalR;
public class BlackJackGameHub(BjRoomManager roomManager) : Hub<IBlackJackGameClient>
{
    public override async  Task OnConnectedAsync() //when a new client connects
    {
        roomManager._clients.Add(Context.ConnectionId);
        await Clients.Caller.UserJoined(Context.ConnectionId);
        await Clients.All.AllClients(roomManager._clients);
        await Clients.Caller.GetAllBjRooms(roomManager.GetRooms() );
    }

    public override async Task OnDisconnectedAsync(Exception exception) //when a client disconnects
    {
        roomManager._clients.Remove(Context.ConnectionId);
        await Clients.All.UserLeft(Context.ConnectionId);
    }

    public async Task PlayerJoinRoom(string roomName)
    {
        var connectionId = Context.ConnectionId;
        var (room, player) = roomManager.AddPlayerToRoom(roomName, connectionId);
        var addToGroupTask = Groups.AddToGroupAsync(connectionId, roomName);  //TODO-HUS aynı oyuncunun 2 farklı yerden odasının yönetilmesi buglara sebep olabilir.
        var playerJoinTask = Clients.Caller.PlayerJoinedRoom(room);
        var updatePlayerTask = Clients.Caller.UpdatePlayer(player);
        await Task.WhenAll(addToGroupTask, playerJoinTask, updatePlayerTask);
    }

    public async Task PlayerLeaveRoom(string roomName)
    {
        var connectionId = Context.ConnectionId;
        var room = roomManager.RemovePlayerFromRoom(roomName, connectionId);
        //TODO-HUS when player is disconnected or leave the room etc. Player should still sit in spot for a 2 rounds.

        await Groups.RemoveFromGroupAsync(connectionId, roomName);
        await Clients.Group(roomName).PlayerLeftRoom(room);
    }

    public async Task SitPlayer(string roomName, int spotId)
    {
        var connectionId = Context.ConnectionId;
        var (room, player) = roomManager.SitPlayerToSpot(roomName, connectionId, spotId);

        var sitPlayerTask =  Clients.Group(roomName).SitPlayer(room); //TODO-HUS burada tum roomu değil sadece oyuncuları update etmeliyiz. UpdateSitingPlayers olabilir.
        var updatePlayerTask = Clients.Caller.UpdatePlayer(player);

        await Task.WhenAll(sitPlayerTask, updatePlayerTask);
    }

    public async Task PlayerLeaveTable(string roomName, int spotId)
    {
        var connectionId = Context.ConnectionId;
        var room = roomManager.RemovePlayerFromSpot(roomName, connectionId, spotId);

        await Clients.Group(roomName).PlayerLeaveTable(room);
    }

    public async Task SendMessage(string connectionId, string groupName)
    {
        //Burada gruba bir mesaj göndermiş olduk.
        await Clients.Group(groupName).SendLog(new() { Message = "here is the message : " + connectionId});
    }

    public async Task PlayerLogin(string userName)
    {
        var userNameIsLogged = PlayerSource.Players.Any(x => x.Name == userName);
        if(userNameIsLogged) return;
        
        PlayerSource.Players.Add(new()
        {
            Id = Context.ConnectionId,
            Name = userName,
            Balance = 1_000_000
        });
        await Clients.Others.SendLog(new() { Message = "New User Joined. Username : " + userName });
    }

    public async Task GetAllBjRooms()
    {
        await Clients.Caller.GetAllBjRooms(roomManager.GetRooms() );
    }
 
    public async Task PlayerBet(string roomName, int spotIndex, decimal betAmount)
    {
        var player = roomManager.GetSittingPlayer(roomName, spotIndex) ?? throw new BjGameException("Koltuk oyuncu yok!!!"); ;
        var game = roomManager.GetGame(roomName);
        
        //player bets
        BalanceManager.PlayerBet(player, game, betAmount, spotIndex);

        //send all players in the room a notification about player's bet
        var bettingNotificationTask = Clients.Group(roomName).PlayerBet(betAmount, spotIndex);
        var updatePlayerTask = Clients.Caller.UpdatePlayer(player);

        //if the table is full and there is no any spot left where bet is not registered, cancel count-down.
        if (game.Table.Spots.Count(x => x.Player is not null) == 7 && !game.Table.Spots.Any(x => x.BetAmount == 0)) {
            game.CancellationTokenSource.Cancel();
        } 
        
        await Task.WhenAll(bettingNotificationTask, updatePlayerTask);
    }

    public Task PlayCardAction(CardAction action, string roomName, int spotNo, bool isForSplittedHand = false)
    {
        var room = roomManager.GetGame(roomName);

        var spot = room.Table.Spots.FirstOrDefault(s => s.Id == spotNo) ?? throw new Exception("Olamamalı la bule bişi");

        if (isForSplittedHand) spot.SplittedHand.NextCardAction = action;
        else spot.Hand.NextCardAction = action;

        // Cancel the timeout when the player makes a move
        room.CancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }
}

// Clients.Caller       --> isteği atan client
// Clients.Others       --> istek atan hariç tüm diğer clientları da hedef olarak seçebiliriz
// Clients.AllExcept    --> Beliritilenler hariç tüm clientler. (connectionId'leri gönder)
// Client.Client        --> Belirtilen connectionId'ye sahip clienta mesaj gönderir.
// Client.Clients       --> Verilen connectionId'lere mesaj gönderir.
// Client.Group         --> Belirtilen gruba subscribe olan tüm clientlara mesaj gönderir.
// Client.GroupExcept   --> Gruptaki clientlar dışındaki gruptaki diğer clientlara mesaj gönderir.
// Client.Groups        --> Birden çok grubtaki clientlara mesaj göndermemizi sağlar. 
// Client.OthersInGroup --> Gruptaki istek atan hariç diğer clientlara mesaj gönderir.
// Client.User          --> Authentication sürecini geçmiş kullanıcıya mesaj göndermeyi sağlar.