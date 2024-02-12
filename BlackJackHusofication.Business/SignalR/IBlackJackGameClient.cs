using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;
using BlackJackHusofication.Model.Models.Notifications;

namespace BlackJackHusofication.Business.SignalR;

public interface IBlackJackGameClient
{
    Task AllClients(List<string> clients);
    Task UserJoined(string connectionId);
    Task UserLeft(string connectionId);
    Task SendLog(SimulationLog logMessage);
    Task GetAllBjRooms(List<string> bjRooms);
    Task PlayerJoinedRoom(BjGame bjRoom);
    Task PlayerLeftRoom(BjGame bjRoom);
    Task SitPlayer(BjGame bjRoom);
    Task PlayerLeaveTable(BjGame bjRoom);
    Task PlayerAction(CardAction action);
    Task PlayerBet(decimal betAmount);
    Task RoundStarted(int roundNumber);
    Task DealCard(Hand hand);

    //Updates
    Task UpdateTable(Table table);
    Task UpdateHand(Hand hand);

    //Notifications
    Task NotifyCountDown(CountDownNotification notification);
    Task NotifyAwaitingCardAction(AwaitingCardActionNotification notification);
    Task NotifyCardDeal(CardDealNotification notification);
}
