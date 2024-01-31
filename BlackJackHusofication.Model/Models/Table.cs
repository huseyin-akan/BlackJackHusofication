namespace BlackJackHusofication.Model.Models;

public class Table
{
    public Dealer Dealer { get; set; }
    public List<Card> Deck { get; set; }
    public List<Card> PlayedCards { get; set; }
    public Card? ShufflerCard { get; set; }
    public decimal Balance { get; set; }
    public List<Spot> Spots{ get; set; }
    public bool IsShoeShouldChange { get; set; }

    public Table()
    {
        Dealer = new() { Id = "Kabazanya", Name = "Husokanus"};
        Spots = [new Spot() { Id = 1}, new Spot() { Id = 2 }, new Spot() { Id = 3 }, new Spot() { Id = 4 },
            new Spot() { Id = 5 }, new Spot() { Id = 6 }, new Spot() { Id = 7 }];
        Deck = [];
        PlayedCards = [];
    }
}
