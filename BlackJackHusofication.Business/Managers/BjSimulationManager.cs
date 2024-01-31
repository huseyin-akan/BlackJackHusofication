using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Business.Services.Abstracts;
using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;
using System.Data;

namespace BlackJackHusofication.Business.Managers;

//TODO-HUS aşağıdaki simulation objesini yeni modele göre yeniden yazalım

//Note: in this mode, 7 players play the most optimal moves. And Husoka bets behind.
public class BjSimulationManager : IGameManager
{
    //private readonly BjGame bjGame; //TODO-HUS aşağıdaki simulation objesi uçacak. Yerine BjRoom gelecek.
    //private readonly BjSimulation _simulation;
    //private readonly IGameLogger loggerService;

    //public BjSimulationManager(IGameLogger loggerService)
    //{
    //    _simulation = new()
    //    {
    //        RoundNo = 0,
    //        Spots = new bool[7],
    //        Players = [],
    //        PlayedCards = [],
    //        Deck = [],
    //        Dealer = new Dealer() { Id = 1.ToString(), Name = "Dealer", Balance = 0 },
    //        Husoka = new Husoka() { Id = 8, HusokaBettedFor = null, Balance = 1_270, Name = "Husoka", CurrentHusokaBet = 0, HusokaIsMorting = false }
    //    };
    //    this.loggerService = loggerService;
    //    bjGame = new() { Name = "SimulationRoom", RoomId = 256};
    //}

    //public async Task StartNewGame()
    //{
    //    SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = "BlackJack oyununa Hoş Geldiniz. Kurpiyeriniz ben Husoka. Tüm paranızı kaybetmeye hazır olun. Hepinizi üteceğim." };
    //    await loggerService.LogMessage(log);

    //    _simulation.Deck = DeckHelper.CreateFullDeck(6);
    //    DeckHelper.ShuffleDecks(_simulation.Deck);
    //    CreateAFulTable();
    //}

    //public async Task PlayRounds(int roundNumber = 1)
    //{
    //    if (roundNumber == 0) return;

    //    for (int i = 0; i < roundNumber; i++)
    //    {
    //        await StartNewRound();
    //        await AskAllPlayersForActions();
    //        await PlayForDealer();
    //        var bettedPlayers = _simulation.
    //        BalanceManager.CheckHandsAndDeliverPrizes(_simulation.Players.Where(x => x.HasBetted), _simulation.Dealer, _simulation.Husoka);
    //        await loggerService.UpdateSimulation(_simulation);
    //        CollectAllCards();
    //    }
    //    await ReportEarnings();
    //}

    //private void CreateAFulTable()
    //{
    //    _simulation.Players.Add(new Player() { Id = 2.ToString(), Name = "Player 1", Balance = 10_000, Spot = GivePlayerSpot() });
    //    _simulation.Players.Add(new Player() { Id = 3.ToString(), Name = "Player 2", Balance = 10_000, Spot = GivePlayerSpot() });
    //    _simulation.Players.Add(new Player() { Id = 4.ToString(), Name = "Player 3", Balance = 10_000, Spot = GivePlayerSpot() });
    //    _simulation.Players.Add(new Player() { Id = 5.ToString(), Name = "Player 4", Balance = 10_000, Spot = GivePlayerSpot() });
    //    _simulation.Players.Add(new Player() { Id = 6.ToString(), Name = "Player 5", Balance = 10_000, Spot = GivePlayerSpot() });
    //    _simulation.Players.Add(new Player() { Id = 7.ToString(), Name = "Player 6", Balance = 10_000, Spot = GivePlayerSpot() });
    //    _simulation.Players.Add(new Player() { Id = 8.ToString(), Name = "Player 7", Balance = 10_000, Spot = GivePlayerSpot() });
    //}

    //private int GivePlayerSpot(int spotNo = 1)
    //{
    //    if (spotNo == 1) spotNo = Array.IndexOf(_simulation.Spots, false) + 1;
    //    if (spotNo == -1) return -1;

    //    _simulation.Spots[spotNo] = true;
    //    return spotNo;
    //}

    //private void CollectAllCards()
    //{
    //    _simulation.PlayedCards.AddRange(_simulation.Dealer.Hand.Cards);
    //    _simulation.Dealer.Hand = new();

    //    foreach (var player in _simulation.Players.Where(x => x.HasBetted))
    //    {
    //        _simulation.PlayedCards.AddRange(player.Hand.Cards);
    //        player.Hand = new();
    //        player.SplittedHand = null;
    //        player.HasBetted = false;
    //    };
    //}

    //private async Task PlayForDealer()
    //{
    //    //If everyone is busted, dealer wins. 
    //    if (!_simulation.Players.Any(x => !x.Hand.IsBusted)
    //        && !_simulation.Players.Any(x => x.SplittedHand is not null && !x.SplittedHand.IsBusted)) return;

    //    SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = $"{_simulation.Dealer.Name}'s second card is: {_simulation.Dealer.Hand.Cards[1].CardType} - {_simulation.Dealer.Hand.Cards[1].CardValue}" };
    //    await loggerService.LogMessage(log);

    //    SimulationLog log2 = new() { LogType = SimulationLogType.GameLog, Message = $"{_simulation.Dealer.Name}'s current hand: {_simulation.Dealer.Hand.HandValue}" };
    //    await loggerService.LogMessage(log2);

    //    //Otherwise dealers opens card and hits until at least 17
    //    while (_simulation.Dealer.Hand.HandValue < 17)
    //    {
    //        DealCard(_simulation.Dealer.Hand);
    //        SimulationLog log3 = new() { LogType = SimulationLogType.DealerActions, Message = $"{_simulation.Dealer.Name} hits. Now the hand is : {_simulation.Dealer.Hand.HandValue}" };
    //        await loggerService.LogMessage(log3);
    //    }
    //}

    //private async Task AskAllPlayersForActions()
    //{
    //    foreach (var player in _simulation.Players.Where(x => x.HasBetted))
    //    {
    //        var shouldAskForNormalHand = true;
    //        while (shouldAskForNormalHand)
    //        {
    //            var playerAction = AskForAction(player.Hand);
    //            shouldAskForNormalHand = await ApplyPlayerAction(player, player.Hand, playerAction);
    //        }
    //        var shouldAskForSplitHand = true;
    //        while (shouldAskForSplitHand)
    //        {
    //            if (player.SplittedHand is null) break;
    //            if (player.SplittedHand.Cards.Count == 1) DealCard(player.SplittedHand);
    //            var playerAction = AskForAction(player.SplittedHand, true);
    //            shouldAskForSplitHand = await ApplyPlayerAction(player, player.SplittedHand, playerAction);
    //        }

    //        if (player.Hand.HandValue > 21) player.Hand.IsBusted = true;
    //    }
    //}

    //private async Task<bool> ApplyPlayerAction(Player player, Hand hand, CardAction playerAction)
    //{
    //    return playerAction switch
    //    {
    //        CardAction.Stand => await ApplyStand(player, hand),
    //        CardAction.Hit => await ApplyHit(player, hand),
    //        CardAction.Double => await ApplyDouble(player, hand),
    //        CardAction.Split => await ApplySplit(player),
    //        _ => throw new Exception("La nasıl olur bu!!!")
    //    };
    //}

    //private async Task<bool> ApplyStand(Player player, Hand hand)
    //{
    //    SimulationLog log3 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} stands. Now the hand is : {hand.HandValue}" };
    //    await loggerService.LogMessage(log3);
    //    return false;
    //}

    //private async Task<bool> ApplySplit(Player player)
    //{
    //    player.SplittedHand = new Hand();
    //    var splitCard = player.Hand.Cards[1];
    //    player.Hand.Cards.Remove(splitCard);
    //    player.SplittedHand.Cards.Add(splitCard);

    //    player.SplittedHand.HandValue = CardManager.GetCountOfHand(player.SplittedHand);
    //    player.Hand.HandValue = CardManager.GetCountOfHand(player.Hand);
    //    SimulationLog log = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} splits the cards. Now the first hand is : {player.Hand.HandValue} and the second hand is : {player.SplittedHand.HandValue}" };
    //    await loggerService.LogMessage(log);
    //    DealCard(player.Hand);
    //    SimulationLog log2 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name}'s new card is {player.Hand.Cards[1].CardValue}. Now the first hand is : {player.Hand.HandValue}" };
    //    await loggerService.LogMessage(log2);
    //    return false;
    //}

    //private async Task<bool> ApplyHit(Player player, Hand hand)
    //{
    //    DealCard(hand);
    //    SimulationLog log2 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} hits. Now the hand is : {hand.HandValue}" };
    //    await loggerService.LogMessage(log2);
    //    return hand.HandValue < 21;
    //}

    //private async Task<bool> ApplyDouble(Player player, Hand hand)
    //{
    //    DealCard(hand);
    //    player.Balance -= hand.BetAmount;
    //    _simulation.Dealer.Balance += hand.BetAmount;
    //    hand.BetAmount *= 2;

    //    SimulationLog log2 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} doubles. Now the hand is : {hand.HandValue}" };
    //    await loggerService.LogMessage(log2);
    //    return false;
    //}

    //private CardAction AskForAction(Hand? hand, bool isSplitHand = false)
    //{
    //    if (hand is null) return CardAction.Stand;
    //    return OptimalMoveHelper.MakeOptimalMove(_simulation.Dealer.Hand.Cards[0], hand, isSplitHand);
    //}

    //private async Task StartNewRound()
    //{
    //    _simulation.RoundNo++;
    //    SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = $"--------------------------- ROUND - {_simulation.RoundNo} HAS STARTED ---------------------------" };
    //    await loggerService.LogMessage(log);
    //    await AcceptTheBets();
    //    DealTheCards();
    //    await WriteTableCardsForRound();
    //}

    //private async Task AcceptTheBets()
    //{
    //    foreach (var player in _simulation.Players.Where(x => x.Spot > 0))
    //    {
    //        player.HasBetted = true;
    //        player.Balance -= 100;
    //        player.Hand.BetAmount = 100;
    //        _simulation.Dealer.Balance += 100;
    //    }

    //    if (!_simulation.Husoka.HusokaIsMorting && _simulation.Players.Any(x => x.NotWinningStreak >= 5))
    //    {
    //        _simulation.Husoka.HusokaIsMorting = true;
    //        if (_simulation.Husoka.CurrentHusokaBet == 0) _simulation.Husoka.CurrentHusokaBet = 10;
    //        else _simulation.Husoka.CurrentHusokaBet *= 2;
    //        _simulation.Husoka.Balance -= _simulation.Husoka.CurrentHusokaBet;
    //        _simulation.Dealer.Balance -= _simulation.Husoka.CurrentHusokaBet;
    //        _simulation.Husoka.HusokaBettedFor = _simulation.Players.First(x => x.NotWinningStreak >= 5);
    //        SimulationLog log = new() { LogType = SimulationLogType.CardDealLog, Message = $"{_simulation.Husoka.Name}'s morting. Let's gooo!. Our balance is : {_simulation.Husoka.Balance}" };
    //        await loggerService.LogMessage(log);
    //    }

    //    else if (_simulation.Husoka.HusokaIsMorting)
    //    {
    //        _simulation.Husoka.CurrentHusokaBet *= 2;
    //        _simulation.Husoka.Balance -= _simulation.Husoka.CurrentHusokaBet;
    //        _simulation.Dealer.Balance -= _simulation.Husoka.CurrentHusokaBet;
    //        SimulationLog log = new() { LogType = SimulationLogType.CardDealLog, Message = $"{_simulation.Husoka.Name}'s mooorting. We bet another {_simulation.Husoka.CurrentHusokaBet} TL. Our balance is : {_simulation.Husoka.Balance}" };
    //        await loggerService.LogMessage(log);
    //    }
    //    await loggerService.UpdateSimulation(_simulation);
    //}

    //private void DealTheCards()
    //{
    //    if (_simulation.IsShoeShouldChange)
    //    {
    //        List<Card> collectedShoe = [.. _simulation.PlayedCards, .. _simulation.Deck, _simulation.ShufflerCard!];
    //        _simulation.Deck = DeckHelper.ShuffleDecks(collectedShoe);
    //        _simulation.PlayedCards = [];
    //        _simulation.ShufflerCard = null;
    //        _simulation.IsShoeShouldChange = false;
    //    }

    //    for (int i = 0; i < 2; i++)
    //    {
    //        //Deal for all players who has betted
    //        foreach (var player in _simulation.Players.Where(x => x.HasBetted)) DealCard(player.Hand);

    //        //Deal for dealer
    //        DealCard(_simulation.Dealer.Hand);
    //    }
    //}

    //private void DealCard(Hand hand)
    //{
    //        var card = _simulation.Deck[0];
    //        _simulation.Deck.Remove(_simulation.Deck[0]);

    //        if (card.CardType == CardType.ShufflerCard)
    //        {
    //            _simulation.IsShoeShouldChange = true;
    //            _simulation.ShufflerCard = card with { };   //copy the card with new reference 
    //            card = _simulation.Deck[0];
    //            _simulation.Deck.Remove(_simulation.Deck[0]);
    //        }

    //        hand.Cards.Add(card);
    //        hand.HandValue = CardManager.GetCountOfHand(hand);

    //        if (hand.HandValue > 21) hand.IsBusted = true;

    //        else if (hand.HandValue == 21 && hand.Cards.Count == 2 && hand.Cards.Any(x => x.CardValue == CardValue.Ace))
    //            hand.IsBlackJack = true;
    //}

    //public async Task WriteAllCards()
    //{
    //    var counter = 0;
    //    foreach (var card in _simulation.Deck)
    //    {
    //        counter++;
    //        await WriteCard(card);
    //    }
    //    Console.WriteLine(counter);
    //}

    //public async Task WriteCard(Card card)
    //{
    //    SimulationLog log = new() { LogType = SimulationLogType.CardDealLog, Message = card.CardValue.ToString() };
    //    SimulationLog log2 = new() { LogType = SimulationLogType.CardDealLog, Message = card.CardType.ToString() };
    //    await loggerService.LogMessage(log);
    //    await loggerService.LogMessage(log2);
    //    Console.WriteLine();
    //}

    //public async Task WriteTableCardsForRound()
    //{
    //    SimulationLog log = new() { LogType = SimulationLogType.DealerActions, Message = "Dealer has :" };
    //    await loggerService.LogMessage(log);
    //    await WriteCard(_simulation.Dealer.Hand.Cards[0]);

    //    foreach (var player in _simulation.Players.Where(x => x.HasBetted))
    //    {
    //        SimulationLog log2 = new() { LogType = SimulationLogType.CardDealLog, Message = player.Name + " has :" };
    //        await loggerService.LogMessage(log2);
    //        await WriteCard(player.Hand.Cards[0]);
    //        await WriteCard(player.Hand.Cards[1]);
    //    }
    //}

    //private async Task ReportEarnings()
    //{
    //    SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = "-----------------------------------------------------------------------------------------" };
    //    await loggerService.LogMessage(log);
    //    foreach (var player in _simulation.Players)
    //    {
    //        SimulationLog log2 = new() { LogType = SimulationLogType.BalanceLog, Message = $"{player.Name} current balance is : {player.Balance}" };
    //        await loggerService.LogMessage(log2);
    //    }
    //    SimulationLog log3 = new() { LogType = SimulationLogType.HusokaLog, Message = $"{_simulation.Husoka.Name} current balance is : {_simulation.Husoka.Balance}" };
    //    await loggerService.LogMessage(log3);
    //    SimulationLog log4 = new() { LogType = SimulationLogType.DealerActions, Message = $"{_simulation.Dealer.Name} current balance is : {_simulation.Dealer.Balance}" };
    //    await loggerService.LogMessage(log4);
    //}

    //public static int AskForRounds()
    //{
    //    bool inputReceived = false;
    //    int numberOfRounds = 0;

    //    while (!inputReceived)
    //    {
    //        Console.Write("Do you want to play another round? (yes/no): ");
    //        string? input = Console.ReadLine();

    //        if (input == "yes")
    //        {
    //            Console.Write("How many rounds do you want to play?: ");
    //            string? numberOfRoundsInput = Console.ReadLine();

    //            if (int.TryParse(numberOfRoundsInput, out int rounds))
    //            {
    //                numberOfRounds = rounds;
    //                inputReceived = true;
    //            }
    //        }
    //        else if (input == "no")
    //        {
    //            inputReceived = true;
    //            numberOfRounds = 0;
    //            Console.WriteLine("Thanks for playing! Exiting the game.");
    //        }
    //        else
    //        {
    //            Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
    //        }
    //    }
    //    return numberOfRounds;
    //}
}