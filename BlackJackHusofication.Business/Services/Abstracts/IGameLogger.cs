using BlackJackHusofication.Model.Logs;

namespace BlackJackHusofication.Business.Services.Abstracts;

public interface IGameLogger
{
    Task LogMessage(SimulationLog logMessage);
}
