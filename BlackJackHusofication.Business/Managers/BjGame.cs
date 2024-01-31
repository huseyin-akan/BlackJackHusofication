using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

public class BjGame
{
    public readonly CancellationTokenSource CancellationTokenSource;
    public required int RoomId { get; set; }
    public required string Name { get; set; }
    public int RoundNo { get; set; }
    public List<Player> Players { get; set; } //Players who are not yet on the table, but those players can bet behind.
    public readonly Table Table;

    public BjGame()
    {
        RoundNo = 0;
        Table = new Table();
        Players = [];
        CancellationTokenSource = new CancellationTokenSource();

        Table.Deck = DeckHelper.CreateFullDeck(6);
        DeckHelper.ShuffleDecks(Table.Deck);
    }

    public void PlayerBet(Player player, decimal betAmount)
    {
        player.HasBetted = true;
        player.Balance -= betAmount;
        Table.Balance += betAmount;
    }
}
