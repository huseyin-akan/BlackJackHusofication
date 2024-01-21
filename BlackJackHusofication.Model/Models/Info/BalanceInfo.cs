namespace BlackJackHusofication.Model.Models.Info;

public class BalanceInfo
{
    public List<PlayerInfo> PlayerInfos { get; set; } = [];
    public DealerInfo DealerInfo { get; set; }
    public HusokaInfo HusokaInfo { get; set; }
}
