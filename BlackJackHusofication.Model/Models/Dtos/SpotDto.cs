namespace BlackJackHusofication.Model.Models.Dtos;

public class SpotDto
{
    public required int Id { get; set; }
    public Hand Hand { get; set; }
    public Hand? SplittedHand { get; set; }
    public decimal BetAmount { get; set; }
    public Player? Player { get; set; }

    public SpotDto()
    {
        Hand = new();
    }
}
