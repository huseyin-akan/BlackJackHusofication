namespace BlackJackHusofication.Model.Models;

public class BjSimulation
{
    public int RoundNo { get; set; }
    public List<Card> Deck { get; set; }
    public List<Card> PlayedCards { get; set; }
    public bool IsShoeShouldChange { get; set; } = false;
    public Card? ShufflerCard { get; set; }
    public  Dealer Dealer { get; set; }
    public  List<Player> Players { get; set; }
    public bool[] Spots { get; set; }
    public Husoka Husoka { get; set; }
}
