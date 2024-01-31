namespace BlackJackHusofication.Model.Models;

public class Player
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public decimal Balance { get; set; }
    public int LosingStreak { get; set; }
    public int NotWinningStreak { get; set; }
    public int WinningStreak { get; set; }
}