using BlackJackHusofication.Business.Managers;
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
    Task PlayerJoinedRoom(BjGame bjRoom);
    Task PlayerLeftRoom(BjGame bjRoom);
    Task SitPlayer(BjGame bjRoom);
    Task PlayerLeaveTable(BjGame bjRoom);
    Task PlayerAction(CardAction action);
    Task PlayerBet(decimal betAmount);
    Task NotifyCountDown(CountDownNotification notification);
}
