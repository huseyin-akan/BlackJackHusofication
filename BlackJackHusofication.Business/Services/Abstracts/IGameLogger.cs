using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;
using BlackJackHusofication.Model.Models.Info;

namespace BlackJackHusofication.Business.Services.Abstracts;

public interface IGameLogger
{
    Task LogMessage(SimulationLog logMessage);

    Task UpdateSimulation(BjSimulation simulation);
}
