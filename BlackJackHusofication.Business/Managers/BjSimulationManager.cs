using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Business.Services.Abstracts;
using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

//Note: in this mode, 7 players play the most optimal moves. And Husoka bets behind.
public class BjSimulationManager  : IGameManager
{
    private int roundNo;
    private List<Card> deck;
    private List<Card> playedCards;
    private bool isShoeShouldChange = false;
    private Card? shufflerCard;
    private readonly Dealer dealer;
    private readonly List<Player> players;
    private readonly bool[] spots;
    private readonly Husoka husoka;

    private readonly IGameLogger _loggerService;

    public BjSimulationManager(IGameLogger loggerService)
    {
        roundNo = 0;
        spots = new bool[8];
        players = [];
        playedCards = [];
        deck = [];
        dealer = new Dealer() { Id = 1, Name = "Dealer", Balance = 0, Spot = GivePlayerSpot(0) };
        husoka = new Husoka() { Id = 8, HusokaBettedFor = null, Balance = 1_270, Name = "Husoka", CurrentHusokaBet = 0, HusokaIsMorting = false };
        _loggerService = loggerService;
    }

    public async Task StartNewGame()
    {
        SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = "BlackJack oyununa Hoş Geldiniz. Kurpiyeriniz ben Husoka. Tüm paranızı kaybetmeye hazır olun. Hepinizi üteceğim." };
        await _loggerService.LogMessage(log);

        deck = DeckHelper.CreateFullDeck(8);
        DeckHelper.ShuffleDecks(deck);
        CreateAFulTable();
    }

    public async Task PlayRounds(int roundNumber = 1)
    {
        if (roundNumber == 0) return;

        for (int i = 0; i < roundNumber; i++)
        {
            await StartNewRound();
            AskAllPlayersForActions();
            PlayForDealer();
            BalanceManager.CheckHandsAndDeliverPrizes(players.Where(x => x.HasBetted), dealer, husoka);
            CollectAllCards();
        }
        ReportEarnings();
    }

    private void CreateAFulTable()
    {
        players.Add(new Player() { Id = 2, Name = "Player 1", Balance = 1_000_000, Spot = GivePlayerSpot() });
        players.Add(new Player() { Id = 3, Name = "Player 2", Balance = 1_000_000, Spot = GivePlayerSpot() });
        players.Add(new Player() { Id = 4, Name = "Player 3", Balance = 1_000_000, Spot = GivePlayerSpot() });
        players.Add(new Player() { Id = 5, Name = "Player 4", Balance = 1_000_000, Spot = GivePlayerSpot() });
        players.Add(new Player() { Id = 6, Name = "Player 5", Balance = 1_000_000, Spot = GivePlayerSpot() });
        players.Add(new Player() { Id = 7, Name = "Player 6", Balance = 1_000_000, Spot = GivePlayerSpot() });
        players.Add(new Player() { Id = 8, Name = "Player 7", Balance = 1_000_000, Spot = GivePlayerSpot() });
    }

    private int GivePlayerSpot(int spotNo = 1)
    {
        if (spotNo == 1) spotNo = Array.IndexOf(spots, false);
        if (spotNo == -1) return -1;

        spots[spotNo] = true;
        return spotNo;
    }

    private void CollectAllCards()
    {
        playedCards.AddRange(dealer.Hand.Cards);
        dealer.Hand = new();

        foreach (var player in players.Where(x => x.HasBetted))
        {
            playedCards.AddRange(player.Hand.Cards);
            player.Hand = new();
            player.SplittedHand = null;
            player.HasBetted = false;
        };
    }

    private async Task PlayForDealer()
    {
        //If everyone is busted, dealer wins. 
        if (!players.Any(x => !x.Hand.IsBusted)
            && !players.Any(x => x.SplittedHand is not null && !x.SplittedHand.IsBusted)) return;

        SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = $"{dealer.Name}'s second card is: {dealer.Hand.Cards[1].CardType} - {dealer.Hand.Cards[1].CardValue}" };
        await _loggerService.LogMessage(log);

        SimulationLog log2 = new() { LogType = SimulationLogType.GameLog, Message = $"{dealer.Name}'s current hand: {dealer.Hand.HandValue}" };
        await _loggerService.LogMessage(log2);

        //Otherwise dealers opens card and hits until at least 17
        while (dealer.Hand.HandValue < 17)
        {
            DealCard(dealer.Hand);
            SimulationLog log3 = new() { LogType = SimulationLogType.DealerActions, Message = $"{dealer.Name} hits. Now the hand is : {dealer.Hand.HandValue}" };
            await _loggerService.LogMessage(log3);
        }
    }

    private async Task AskAllPlayersForActions()
    {
        foreach (var player in players.Where(x => x.HasBetted))
        {
            var shouldAskForNormalHand = true;
            while (shouldAskForNormalHand)
            {
                var playerAction = AskForAction(player.Hand);
                shouldAskForNormalHand = await ApplyPlayerAction(player, player.Hand, playerAction);
            }
            var shouldAskForSplitHand = true;
            while (shouldAskForSplitHand)
            {
                if (player.SplittedHand is null) break;
                if (player.SplittedHand.Cards.Count == 1) DealCard(player.SplittedHand);
                var playerAction = AskForAction(player.SplittedHand, true);
                shouldAskForSplitHand = await ApplyPlayerAction(player, player.SplittedHand, playerAction);
            }

            if (player.Hand.HandValue > 21) player.Hand.IsBusted = true;
        }
    }

    private async Task<bool> ApplyPlayerAction(Player player, Hand hand, CardAction playerAction)
    {
        return playerAction switch
        {
            CardAction.Stand => await ApplyStand(player, hand),
            CardAction.Hit => await ApplyHit(player, hand),
            CardAction.Double => await ApplyDouble(player, hand),
            CardAction.Split => await ApplySplit(player),
            _ => throw new Exception("La nasıl olur bu!!!")
        };
    }

    private async Task<bool> ApplyStand(Player player, Hand hand)
    {
        SimulationLog log3 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} stands. Now the hand is : {hand.HandValue}" };
        await _loggerService.LogMessage(log3);
        return false;
    }

    private async Task<bool> ApplySplit(Player player)
    {
        player.SplittedHand = new Hand();
        var splitCard = player.Hand.Cards[1];
        player.Hand.Cards.Remove(splitCard);
        player.SplittedHand.Cards.Add(splitCard);

        player.SplittedHand.HandValue = CardManager.GetCountOfHand(player.SplittedHand);
        player.Hand.HandValue = CardManager.GetCountOfHand(player.Hand);
        SimulationLog log = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} splits the cards. Now the first hand is : {player.Hand.HandValue} and the second hand is : {player.SplittedHand.HandValue}" };
        await _loggerService.LogMessage(log);
        DealCard(player.Hand);
        SimulationLog log2 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name}'s new card is {player.Hand.Cards[1].CardValue}. Now the first hand is : {player.Hand.HandValue}" };
        await _loggerService.LogMessage(log2);
        return true;
    }

    private async Task<bool> ApplyHit(Player player, Hand hand)
    {
        DealCard(hand);
        SimulationLog log2 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} hits. Now the hand is : {hand.HandValue}" };
        await _loggerService.LogMessage(log2);
        return hand.HandValue < 21;
    }

    private async Task<bool> ApplyDouble(Player player, Hand hand)
    {
        DealCard(hand);
        player.Balance -= hand.BetAmount;
        dealer.Balance += hand.BetAmount;
        hand.BetAmount *= 2;

        SimulationLog log2 = new() { LogType = SimulationLogType.CardActionLog, Message = $"{player.Name} doubles. Now the hand is : {hand.HandValue}" };
        await _loggerService.LogMessage(log2);
        return false;
    }

    private CardAction AskForAction(Hand? hand, bool isSplitHand = false)
    {
        if (hand is null) return CardAction.Stand;
        return OptimalMoveManager.MakeOptimalMove(dealer.Hand.Cards[0], hand, isSplitHand);
    }

    private async Task StartNewRound()
    {
        roundNo++;
        SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = $"--------------------------- ROUND - {roundNo} HAS STARTED ---------------------------" };
        await _loggerService.LogMessage(log);
        await AcceptTheBets();
        DealTheCards();
        WriteTableCardsForRound();
    }

    private async Task AcceptTheBets()
    {
        foreach (var player in players.Where(x => x.Spot > 0))
        {
            player.HasBetted = true;
            player.Balance -= 100;
            player.Hand.BetAmount = 100;
            dealer.Balance += 100;
        }

        if (!husoka.HusokaIsMorting && players.Any(x => x.NotWinningStreak >= 5))
        {
            husoka.HusokaIsMorting = true;
            if (husoka.CurrentHusokaBet == 0) husoka.CurrentHusokaBet = 10;
            else husoka.CurrentHusokaBet *= 2;
            husoka.Balance -= husoka.CurrentHusokaBet;
            dealer.Balance -= husoka.CurrentHusokaBet;
            husoka.HusokaBettedFor = players.First(x => x.NotWinningStreak >= 5);
            SimulationLog log = new() { LogType = SimulationLogType.CardDealLog, Message = $"{husoka.Name}'s morting. Let's gooo!. Our balance is : {husoka.Balance}" };
            await _loggerService.LogMessage(log);
        }

        else if (husoka.HusokaIsMorting)
        {
            husoka.CurrentHusokaBet *= 2;
            husoka.Balance -= husoka.CurrentHusokaBet;
            dealer.Balance -= husoka.CurrentHusokaBet;
            SimulationLog log = new() { LogType = SimulationLogType.CardDealLog, Message = $"{husoka.Name}'s mooorting. We bet another {husoka.CurrentHusokaBet} TL. Our balance is : {husoka.Balance}" };
            await _loggerService.LogMessage(log);
        }
    }

    private void DealTheCards()
    {
        if (isShoeShouldChange)
        {
            List<Card> collectedShoe = [.. playedCards, .. deck, shufflerCard!];
            deck = DeckHelper.ShuffleDecks(collectedShoe);
            playedCards = [];
            shufflerCard = null;
            isShoeShouldChange = false;
        }

        for (int i = 0; i < 2; i++)
        {
            //Deal for all players who has betted
            foreach (var player in players.Where(x => x.HasBetted)) DealCard(player.Hand);

            //Deal for dealer
            DealCard(dealer.Hand);
        }
    }

    private void DealCard(Hand hand)
    {
        var card = deck[0];
        deck.Remove(deck[0]);

        if (card.CardType == CardType.ShufflerCard)
        {
            isShoeShouldChange = true;
            shufflerCard = card with { };   //copy the card with new reference
            card = deck[0];
            deck.Remove(deck[0]);
        }

        hand.Cards.Add(card);
        hand.HandValue = CardManager.GetCountOfHand(hand);

        if (hand.HandValue > 21) hand.IsBusted = true;

        else if (hand.HandValue == 21 && hand.Cards.Count == 2 && hand.Cards.Any(x => x.CardValue == CardValue.Ace))
            hand.IsBlackJack = true;
    }

    public async Task WriteAllCards()
    {
        var counter = 0;
        foreach (var card in deck)
        {
            counter++;
            await WriteCard(card);
        }
        Console.WriteLine(counter);
    }

    public async Task WriteCard(Card card)
    {
        ConsoleColor color = card.CardType switch
        {
            CardType.Spades or CardType.Clubs => ConsoleColor.DarkGray,
            CardType.Hearts or CardType.Diamonds => ConsoleColor.Red,
            CardType.ShufflerCard => ConsoleColor.Cyan,
            _ => throw new NotImplementedException("Unhandled CardType"),
        };

        SimulationLog log = new() { LogType = SimulationLogType.CardDealLog, Message = card.CardValue.ToString() };
        SimulationLog log2 = new() { LogType = SimulationLogType.CardDealLog, Message = card.CardType.ToString() };
        await _loggerService.LogMessage(log);
        await _loggerService.LogMessage(log2);
        Console.WriteLine();
    }

    public async Task WriteTableCardsForRound()
    {
        SimulationLog log = new() { LogType = SimulationLogType.DealerActions, Message = "Dealer has :" };
        await _loggerService.LogMessage(log);
        await WriteCard(dealer.Hand.Cards[0]);

        foreach (var player in players.Where(x => x.HasBetted))
        {
            SimulationLog log2 = new() { LogType = SimulationLogType.CardDealLog, Message = player.Name + " has :" };
            await _loggerService.LogMessage(log2);
            await WriteCard(player.Hand.Cards[0]);
            await WriteCard(player.Hand.Cards[1]);
        }
    }

    private async Task ReportEarnings()
    {
        SimulationLog log = new() { LogType = SimulationLogType.GameLog, Message = "-----------------------------------------------------------------------------------------" };
        await _loggerService.LogMessage(log);
        foreach (var player in players)
        {
            SimulationLog log2 = new() { LogType = SimulationLogType.BalanceLog, Message = $"{player.Name} current balance is : {player.Balance}" };
            await _loggerService.LogMessage(log2);
        }
        SimulationLog log3 = new() { LogType = SimulationLogType.HusokaLog, Message = $"{husoka.Name} current balance is : {husoka.Balance}" };
        await _loggerService.LogMessage(log3);
        SimulationLog log4 = new() { LogType = SimulationLogType.DealerActions, Message = $"{dealer.Name} current balance is : {dealer.Balance}" };
        await _loggerService.LogMessage(log4);
    }

    public static int AskForRounds()
    {
        bool inputReceived = false;
        int numberOfRounds = 0;

        while (!inputReceived)
        {
            Console.Write("Do you want to play another round? (yes/no): ");
            string? input = Console.ReadLine();

            if (input == "yes")
            {
                Console.Write("How many rounds do you want to play?: ");
                string? numberOfRoundsInput = Console.ReadLine();

                if (int.TryParse(numberOfRoundsInput, out int rounds))
                {
                    numberOfRounds = rounds;
                    inputReceived = true;
                }
            }
            else if (input == "no")
            {
                inputReceived = true;
                numberOfRounds = 0;
                Console.WriteLine("Thanks for playing! Exiting the game.");
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }
        }
        return numberOfRounds;
    }
}