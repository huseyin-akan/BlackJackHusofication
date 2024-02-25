namespace BlackJackHusofication.Model.Models.Dtos;

public class DealerDto
{
    public DealerDto()
    {
        Hand = new();
    }
    public required string Id { get; set; }
    public required string Name { get; set; }
    public Hand Hand { get; set; }
}
