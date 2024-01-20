namespace BlackJackHusofication.Model.Models;

public class Player
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
    public int NotWinningStreak { get; set; }
    public int WinningStreak { get; set; }
    public bool HasBetted { get; set; }

    public Hand Hand { get; set; }
    public Hand? SplittedHand { get; set; }
}