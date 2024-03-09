using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

public class BjGame
{
    public CancellationTokenSource CancellationTokenSource;
    public required int RoomId { get; set; }
    public required string Name { get; set; }
    public int RoundNo { get; set; }
    public List<Player> Players { get; set; } //Players who are not yet on the table, but those players can bet behind.
    public Table Table { get; }
    public bool IsAcceptingBets { get; set; }
    //public bool IsDealingAllCards { get; set; }
    //public bool IsAskingForActions { get; set; }
    public BjGame()
    {
        RoundNo = 0;
        Table = new Table();
        Players = [];
        CancellationTokenSource = new CancellationTokenSource();

        Table.Deck = DeckHelper.CreateFullDeck(6);
        DeckHelper.ShuffleDecks(Table.Deck);
    }
}