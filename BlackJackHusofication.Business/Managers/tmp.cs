//using BlackJackHusofication.Business.Helpers;
//using BlackJackHusofication.Business.Services.Abstracts;
//using BlackJackHusofication.Model.Logs;
//using BlackJackHusofication.Model.Models;
//using System.Data;

//namespace BlackJackHusofication.Business.Managers;

////Note: in this mode, 7 players play the most optimal moves. And Husoka bets behind.
//public class BjSimulationManager2 : IGameManager
//{
//    private readonly BjRoom bjRoom; //TODO-HUS aşağıdaki simulation objesi uçacak. Yerine BjRoom gelecek.
//    private readonly BjSimulation _simulation;
//    private readonly IGameLogger loggerService;

//    public BjSimulationManager2(IGameLogger loggerService)
//    {
//        _simulation = new()
//        {
//            RoundNo = 0,
//            Spots = new bool[7],
//            Players = [],
//            PlayedCards = [],
//            Deck = [],
//            Dealer = new Dealer() { Id = 1.ToString(), Name = "Dealer", Balance = 0 },
//            Husoka = new Husoka() { Id = 8, HusokaBettedFor = null, Balance = 1_270, Name = "Husoka", CurrentHusokaBet = 0, HusokaIsMorting = false }
//        };
//        this.loggerService = loggerService;
//        bjRoom = new() { Name = "SimulationRoom", RoomId = 256 };
//    }

//    public async Task PlayRounds(int roundNumber = 1)
//    {
        
//        await PlayForDealer();
//        BalanceManager.CheckHandsAndDeliverPrizes(_simulation.Players.Where(x => x.HasBetted), _simulation.Dealer, _simulation.Husoka);
//        await loggerService.UpdateSimulation(_simulation);
//        CollectAllCards();
//        await ReportEarnings();
//    }

//    private int GivePlayerSpot(int spotNo = 1)
//    {
//        if (spotNo == 1) spotNo = Array.IndexOf(_simulation.Spots, false) + 1;
//        if (spotNo == -1) return -1;

//        _simulation.Spots[spotNo] = true;
//        return spotNo;
//    }

//    private void CollectAllCards()
//    {
//        _simulation.PlayedCards.AddRange(_simulation.Dealer.Hand.Cards);
//        _simulation.Dealer.Hand = new();

//        foreach (var player in _simulation.Players.Where(x => x.HasBetted))
//        {
//            _simulation.PlayedCards.AddRange(player.Hand.Cards);
//            player.Hand = new();
//            player.SplittedHand = null;
//            player.HasBetted = false;
//        };
//    }

//    private async Task PlayForDealer()
//    {
//        //If everyone is busted, dealer wins. 
//        if (!_simulation.Players.Any(x => !x.Hand.IsBusted)
//            && !_simulation.Players.Any(x => x.SplittedHand is not null && !x.SplittedHand.IsBusted)) return;

//        SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = $"{_simulation.Dealer.Name}'s second card is: {_simulation.Dealer.Hand.Cards[1].CardType} - {_simulation.Dealer.Hand.Cards[1].CardValue}" };
//        await loggerService.LogMessage(log);

//        SimulationLog log2 = new() { LogType = SimulationLogType.GameLog, Message = $"{_simulation.Dealer.Name}'s current hand: {_simulation.Dealer.Hand.HandValue}" };
//        await loggerService.LogMessage(log2);

//        //Otherwise dealers opens card and hits until at least 17
//        while (_simulation.Dealer.Hand.HandValue < 17)
//        {
//            DealCard(_simulation.Dealer.Hand);
//            SimulationLog log3 = new() { LogType = SimulationLogType.DealerActions, Message = $"{_simulation.Dealer.Name} hits. Now the hand is : {_simulation.Dealer.Hand.HandValue}" };
//            await loggerService.LogMessage(log3);
//        }
//    }

    

    

//    private async Task ReportEarnings()
//    {
//        SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = "-----------------------------------------------------------------------------------------" };
//        await loggerService.LogMessage(log);
//        foreach (var player in _simulation.Players)
//        {
//            SimulationLog log2 = new() { LogType = SimulationLogType.BalanceLog, Message = $"{player.Name} current balance is : {player.Balance}" };
//            await loggerService.LogMessage(log2);
//        }
//        SimulationLog log3 = new() { LogType = SimulationLogType.HusokaLog, Message = $"{_simulation.Husoka.Name} current balance is : {_simulation.Husoka.Balance}" };
//        await loggerService.LogMessage(log3);
//        SimulationLog log4 = new() { LogType = SimulationLogType.DealerActions, Message = $"{_simulation.Dealer.Name} current balance is : {_simulation.Dealer.Balance}" };
//        await loggerService.LogMessage(log4);
//    }

//    public static int AskForRounds()
//    {
//        bool inputReceived = false;
//        int numberOfRounds = 0;

//        while (!inputReceived)
//        {
//            Console.Write("Do you want to play another round? (yes/no): ");
//            string? input = Console.ReadLine();

//            if (input == "yes")
//            {
//                Console.Write("How many rounds do you want to play?: ");
//                string? numberOfRoundsInput = Console.ReadLine();

//                if (int.TryParse(numberOfRoundsInput, out int rounds))
//                {
//                    numberOfRounds = rounds;
//                    inputReceived = true;
//                }
//            }
//            else if (input == "no")
//            {
//                inputReceived = true;
//                numberOfRounds = 0;
//                Console.WriteLine("Thanks for playing! Exiting the game.");
//            }
//            else
//            {
//                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
//            }
//        }
//        return numberOfRounds;
//    }
//}