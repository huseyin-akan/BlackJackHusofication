namespace BlackJackHusofication.Model.Models;

public class BjRoom
{
    public CancellationTokenSource _cancellationTokenSource;
    public required int RoomId { get; set; }
    public required string Name { get; set; }
    public int RoundNo { get; set; }
    public List<Player> Players { get; set; } //Players who are not yet on the table, but those players can bet behind.
    public Table Table { get; set; }
    public bool IsAcceptingBets { get; set; }

    public BjRoom()
    {
        RoundNo = 0;
        Table = new Table();
        Players = [];
        IsAcceptingBets = false;
        _cancellationTokenSource = new CancellationTokenSource();
    }
}
