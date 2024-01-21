using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.SignalR;

public interface IBlackJackClient
{
    Task AllClients(List<string> clients);
    Task UserJoined(string connectionId);
    Task UserLeft(string connectionId);
    Task UpdateSimulation(BjSimulation simulation);
    Task SendLog(SimulationLog logMessage);
}
