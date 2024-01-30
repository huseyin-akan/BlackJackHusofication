using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

public class BjRoomManager
{
    private readonly Dictionary<string, BjRoom> _rooms = [];
    public readonly List<string> _clients = [];

    public BjRoomManager()
    {
        _rooms.Add("Blackjack - 1", new(){ RoomId = 1, Name = "Blackjack - 1" });
        _rooms.Add("Blackjack - 2", new(){ RoomId = 1, Name = "Blackjack - 2" });
        _rooms.Add("Blackjack - 3", new(){ RoomId = 1, Name = "Blackjack - 3" });
        _rooms.Add("Blackjack - 4", new(){ RoomId = 1, Name = "Blackjack - 4" });
        _rooms.Add("Blackjack - 5", new(){ RoomId = 1, Name = "Blackjack - 5" });
        _rooms.Add("Blackjack - 6", new(){ RoomId = 1, Name = "Blackjack - 6" });
        _rooms.Add("Blackjack - 7", new(){ RoomId = 1, Name = "Blackjack - 7" });
        _rooms.Add("Blackjack - 8", new(){ RoomId = 1, Name = "Blackjack - 8" });
        _rooms.Add("Blackjack - 9", new(){ RoomId = 1, Name = "Blackjack - 9" });
        _rooms.Add("Blackjack - 10", new(){ RoomId = 1, Name = "Blackjack - 10" });
        _rooms.Add("Blackjack - 11", new(){ RoomId = 1, Name = "Blackjack - 11" });
    }

    public void CreateRoom(string roomName, int roomId)
    {
        _rooms[roomName] = new BjRoom() { Name = roomName, RoomId = roomId };
    }

    public BjRoom AddPlayerToRoom(string roomName, string connectionId)
    {
        var room = _rooms[roomName] ?? throw new Exception("Böyle bir oda yok la!!!"); 

        Player newPlayer = new() { Id = connectionId, Name = "Husoman" }; //TODO-HUS oyuncu adı.
        room.Players.Add(newPlayer);
        return room;
    }

    public BjRoom RemovePlayer(string roomName, string connectionId)
    {
        var room = _rooms[roomName] ?? throw new Exception("Böyle bir oda yok la!!!");

        var currentPlayer = room.Players.FirstOrDefault(p => p.Id == connectionId)
            ?? throw new Exception("Böyle bir oyuncu yok kardeşim");

        room.Players.Remove(currentPlayer);
        room.Table.Players.Remove(currentPlayer);
        return room;
    }

    public BjRoom SitPlayerToTable(string roomName, string connectionId, int spotIndex)
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

    public BjRoom RemovePlayerFromTable(string roomName, string connectionId)
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

    public BjRoom GetRoom(string roomName) => _rooms[roomName] ?? throw new Exception("Oda yok");
}