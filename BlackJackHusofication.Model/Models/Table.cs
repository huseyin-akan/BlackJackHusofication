namespace BlackJackHusofication.Model.Models;

public class Table
{
    public Dealer Dealer { get; set; }
    public List<Card> Deck { get; set; }
    public List<Card> PlayedCards { get; set; }
    public Card? ShufflerCard { get; set; }
    public List<Player> Players { get; set; }
    public decimal Balance { get; set; }
    public bool[] Spots{ get; set; }
    public bool IsShoeShouldChange { get; set; }

    public Table()
    {
        Dealer = new() { Id = "Kabazanya", Name = "Husokanus"};
        Players = [];
        Spots = new bool[7];
        Deck = [];
        PlayedCards = [];
    }
}
