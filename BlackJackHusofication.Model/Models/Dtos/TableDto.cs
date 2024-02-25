using BlackJackHusofication.Model.Models.Dtos;

namespace BlackJackHusofication.Model.Models;

public class TableDto
{
    public DealerDto Dealer { get; set; }
    public Card? ShufflerCard { get; set; }
    public decimal Balance { get; set; }
    public List<Spot> Spots{ get; set; }
    public bool IsShoeShouldChange { get; set; }

    public TableDto()
    {
        Dealer = new() { Id = "Kabazanya", Name = "Husokanus"};
        Spots = [new Spot() { Id = 1}, new Spot() { Id = 2 }, new Spot() { Id = 3 }, new Spot() { Id = 4 },
            new Spot() { Id = 5 }, new Spot() { Id = 6 }, new Spot() { Id = 7 }];
    }
}
