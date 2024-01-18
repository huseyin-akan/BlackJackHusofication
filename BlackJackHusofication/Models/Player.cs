
namespace BlackJackHusofication.Models;

internal class Player
{
    public Player()
    {
        Hand = new();
    }
    public required int Id { get; set; }
    public required string Name { get; set; }
    public int Spot { get; set; }
    public decimal Balance { get; set; }
    public int LosingStreak { get; set; }
    public bool HasBetted { get; set; }

    public Hand Hand {get; set;}
    public Hand? SplittedHand {get; set;}
}