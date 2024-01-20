namespace BlackJackHusofication.Model.Logs;

public class SimulationLog
{
    public SimulationLogType LogType { get; set; }
    public required string Message { get; set; }
}

public enum SimulationLogType
{
    CardActionLog = 1,
    CardDealLog = 2,
    GameLog = 3,
    DealerActions = 4,
    BalanceLog = 5,
    HusokaLog = 6
}
