using BlackJackHusofication.Business.Mappings;
using BlackJackHusofication.Model.Exceptions;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

public class BjRoomManager
{
    private readonly Dictionary<string, BjGame> _rooms = [];
    public readonly List<string> _clients = [];

    public BjGame CreateRoom(string roomName, int roomId)
    {
        return _rooms[roomName] = new BjGame() { Name = roomName, RoomId = roomId };
    }

    public (BjGameDto game, Player player) AddPlayerToRoom(string roomName, string connectionId)
    {
        var room = _rooms[roomName] ?? throw new Exception("Böyle bir oda yok la!!!"); 

        Player newPlayer = new() { Id = connectionId, Name = "Husoman", Balance = 5000 }; //TODO-HUS oyuncu adı.
        room.Players.Add(newPlayer);
        var mapped = MappingHelper.MapBjGameObject(room);
        return (mapped , newPlayer);
    }
    
    public BjGame RemovePlayerFromRoom(string roomName, string connectionId)
    {
        var room = _rooms[roomName] ?? throw new BjGameException("Böyle bir oda yok la!!!");

        var currentPlayer = room.Players.FirstOrDefault(p => p.Id == connectionId)
            ?? throw new BjGameException("Böyle bir oyuncu yok kardeşim");

        room.Players.Remove(currentPlayer);
        var playerSittingSpots = room.Table.Spots.Where(x => x?.Player?.Id == currentPlayer.Id);
        foreach (var spot in playerSittingSpots)
        {
            spot.Player = null;
        }

        return room;
    }

    //TODO-HUS Global Exception yazalım SignalR için de. Sonra frontend tarafında notification verelim.
    public (BjGame room, Player player) SitPlayerToSpot(string roomName, string connectionId, int spotId)
    {
        var room = _rooms[roomName] ?? throw new BjGameException("Böyle bir oda yok la!!!");
        var currentPlayer = room.Players.FirstOrDefault(p => p.Id == connectionId)
            ?? throw new BjGameException("Böyle bir oyuncu yok kardeşim");

        var spot = room.Table.Spots.FirstOrDefault(x => x.Id == spotId)
            ?? throw new BjGameException("Oturmak isteği attığınız koltuk mevcut değil!!!");
        var isSpotAvailable = spot.Player is null;
        
        if (!isSpotAvailable) throw new BjGameException("Yer yok la nasıl oturacan :D ");
        else spot.Player = currentPlayer;
        
        return (room, currentPlayer);
    }

    public BjGame RemovePlayerFromSpot(string roomName, string connectionId, int spotId)
    {
        var room = _rooms[roomName] ?? throw new BjGameException("Böyle bir oda yok la!!!");
        var currentPlayer = room.Players.FirstOrDefault(p => p.Id == connectionId)
            ?? throw new Exception("Böyle bir oyuncu yok kardeşim");

        var spot = room.Table.Spots.FirstOrDefault(x => x.Id == spotId)
            ?? throw new BjGameException("Oturma isteği attığınız koltuk mevcut değil!!!");

        spot.Player = null;

        return room;
    }

    public List<string> GetRooms() => [.. _rooms.Keys];

    public BjGame GetGame(string roomName) => _rooms[roomName] ?? throw new BjGameException("Oda yok");

    public Player? GetSittingPlayer(string roomName, int spotId)
    {
        var spot = _rooms[roomName].Table.Spots.FirstOrDefault(x => x.Id == spotId)
            ?? throw new BjGameException("Oturma isteği attığınız koltuk mevcut değil!!!");

        return spot.Player;
    } 
}