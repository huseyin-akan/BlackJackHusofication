using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.DataAccess.StaticData;

public class BjRoomSource
{
    public static List<BjRoom> Rooms { get; } = [
        new BjRoom() { RoomId = 1, Name = "Blackjack - 1" },
        new BjRoom() { RoomId = 2, Name = "Blackjack - 2" },
        new BjRoom() { RoomId = 3, Name = "Blackjack - 3" },
        new BjRoom() { RoomId = 4, Name = "Blackjack - 4" },
        new BjRoom() { RoomId = 5, Name = "Blackjack - 5" },
        new BjRoom() { RoomId = 6, Name = "Blackjack - 6" },
        new BjRoom() { RoomId = 7, Name = "Blackjack - 7" },
        new BjRoom() { RoomId = 8, Name = "Blackjack - 8" },
        new BjRoom() { RoomId = 9, Name = "Blackjack - 9" },
        new BjRoom() { RoomId = 10, Name = "Blackjack - 10" },
        new BjRoom() { RoomId = 11, Name = "Blackjack - 11" },
        new BjRoom() { RoomId = 12, Name = "Blackjack - 12" },
        new BjRoom() { RoomId = 13, Name = "Blackjack - 13" },
    ];
}
