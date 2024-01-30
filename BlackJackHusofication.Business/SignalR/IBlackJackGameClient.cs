using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.SignalR;

public interface IBlackJackGameClient
{
    Task AllClients(List<string> clients);
    Task UserJoined(string connectionId);
    Task UserLeft(string connectionId);
    Task UpdateSimulation(BjSimulation simulation);
    Task SendLog(SimulationLog logMessage);
    Task GetAllBjRooms(List<string> bjRooms);
    Task PlayerJoinedRoom(BjRoom bjRoom);
    Task PlayerLeftRoom(BjRoom bjRoom);
    Task SitPlayer(BjRoom bjRoom);
    Task PlayerLeaveTable(BjRoom bjRoom);
    Task PlayerAction(CardAction action);
}
