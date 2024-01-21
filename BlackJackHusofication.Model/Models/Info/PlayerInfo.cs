namespace BlackJackHusofication.Model.Models.Info;

public class PlayerInfo
{
    public required string Name { get; set; }
    public required decimal Balance { get; set; }
    public required int LosingStreak { get; set; }
    public required int WinningStreak { get; set; }
}
