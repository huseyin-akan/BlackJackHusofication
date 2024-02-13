using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

public class BjGameDto
{
    public required int RoomId { get; set; }
    public required string Name { get; set; }
    public int RoundNo { get; set; }
    public TableDto Table { get; }

    public BjGameDto()
    {
        RoundNo = 0;
        Table = new TableDto();
    }
}