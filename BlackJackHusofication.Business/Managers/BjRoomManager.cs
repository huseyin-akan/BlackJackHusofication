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

    public BjGame AddPlayerToRoom(string roomName, string connectionId)
    {
        var room = _rooms[roomName] ?? throw new Exception("Böyle bir oda yok la!!!"); 

        Player newPlayer = new() { Id = connectionId, Name = "Husoman" }; //TODO-HUS oyuncu adı.
        room.Players.Add(newPlayer);
        return room;
    }

    public BjGame RemovePlayer(string roomName, string connectionId)
    {
        var room = _rooms[roomName] ?? throw new Exception("Böyle bir oda yok la!!!");

        var currentPlayer = room.Players.FirstOrDefault(p => p.Id == connectionId)
            ?? throw new Exception("Böyle bir oyuncu yok kardeşim");

        room.Players.Remove(currentPlayer);
        room.Table.Players.Remove(currentPlayer);
        return room;
    }

    public BjGame SitPlayerToTable(string roomName, string connectionId, int spotIndex)
    {
        var room = _rooms[roomName] ?? throw new Exception("Böyle bir oda yok la!!!");
        var currentPlayer = room.Players.FirstOrDefault(p => p.Id == connectionId)
            ?? throw new Exception("Böyle bir oyuncu yok kardeşim");

        var isSpotAvailable = room.Table.Spots[spotIndex];
        if (isSpotAvailable) throw new Exception("Dolu kucağa oturmak bizde yasak la"); ;

        currentPlayer.Spot = spotIndex;
        room.Table.Players.Add(currentPlayer);
        room.Table.Spots[spotIndex] = true;

        return room;
    }

    public BjGame RemovePlayerFromTable(string roomName, string connectionId)
    {
        var room = _rooms[roomName] ?? throw new Exception("Böyle bir oda yok la!!!");
        var currentPlayer = room.Table.Players.FirstOrDefault(p => p.Id == connectionId)
            ?? throw new Exception("Böyle bir oyuncu yok kardeşim");

        room.Table.Players.Remove(currentPlayer);
        room.Table.Spots[currentPlayer.Spot] = false;
        currentPlayer.Spot = 0;

        return room;
    }

    public List<string> GetRooms() => [.. _rooms.Keys];

    public BjGame GetGame(string roomName) => _rooms[roomName] ?? throw new Exception("Oda yok");

    public Player GetSittingPlayer(string roomName, string connectionId) =>
        _rooms[roomName].Table.Players.FirstOrDefault(x => x.Id == connectionId) ?? throw new Exception("Bu odada bu oyuncu masada oturmuyor.");
}