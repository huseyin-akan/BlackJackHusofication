using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Model.Models;
using BlackJackHusofication.Model.Models.Dtos;

namespace BlackJackHusofication.Business.Mappings;

public class MappingHelper
{
    public static BjGameDto MapBjGameObject(BjGame game)
    {
        return new BjGameDto
        {
            Name = game.Name,
            RoomId = game.RoomId,
            RoundNo = game.RoundNo,
            Table = MapTableObject(game.Table)
        };
    }

    public static TableDto MapTableObject(Table table)
    {
        return new TableDto
        {
            Balance = table.Balance,
            Dealer = MapDealerObject(table.Dealer),
            IsShoeShouldChange = table.IsShoeShouldChange,
            ShufflerCard = table.ShufflerCard,
            Spots = table.Spots
        };
    }

    public static DealerDto MapDealerObject(Dealer dealer)
    {
        return new DealerDto
        {
            Id = dealer.Id,
            Name = dealer.Name,
            Hand = dealer.Hand
        };
    }
}
