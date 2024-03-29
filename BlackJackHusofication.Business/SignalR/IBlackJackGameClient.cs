﻿using BlackJackHusofication.Business.Managers;
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
    Task PlayerJoinedRoom(BjGameDto bjRoom);
    Task PlayerLeftRoom(BjGame bjRoom); //TODO-HUS BjGameDto olmalı sanırım
    Task PlayerLeaveSpot(BjGame bjRoom); //TODO-HUS BjGameDto olmalı sanırım
    Task PlayerAction(CardAction action);
    Task PlayerBet(decimal betAmount, int spotIndex);
    Task RoundStarted(int roundNumber);
    Task DealCard(Hand hand);

    //Updates
    Task UpdateTable(TableDto table);
    Task UpdateHand(Hand hand);
    Task UpdatePlayer(Player player);
    Task UpdateSpots(List<Spot> spots);  

    //Notifications
    Task NotifyCountDown(CountDownNotification notification);
    Task NotifyAwaitingCardAction(AwaitingCardActionNotification notification);
    Task NotifyCardDeal(CardDealNotification notification);
    Task NotifySecretCard(SecretCardNotification notification);
    Task NotifyRoundWinnigs(RoundWinningsNotification notification);
}
