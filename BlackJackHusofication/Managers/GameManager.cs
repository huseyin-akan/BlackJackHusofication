using BlackJackHusofication.Helpers;
using BlackJackHusofication.Models;
using System.Numerics;

namespace BlackJackHusofication.Managers;

internal class GameManager
{
    private int roundNo;
    private List<Card> deck;
    private List<Card> playedCards;
    private readonly Dealer dealer;
    private readonly List<Player> players;
    private readonly Player husoka;
    private Player? husokaBettedFor;
    private readonly bool[] spots;
    private bool husokaIsMorting = false;
    private decimal currentHusokaBet = 0;
    private bool isShoeShouldChange = false;
    private Card? shufflerCard;

    public GameManager()
    {
        roundNo = 0;
        spots = new bool[8];
        players = [];
        playedCards = [];
        deck = [];
        dealer = new Dealer() { Id = 1, Name = "Dealer", Balance = 0, Spot = GivePlayerSpot(0) };
        husoka = new Player() { Id = 8, Name = "Husoka", Balance = 1_270 };

        StartNewGame();
    }

    public void StartNewGame()
    {
        ConsoleHelper.WriteLine("BlackJack oyununa Hoş Geldiniz. Kurpiyeriniz ben Husoka. Tüm paranızı kaybetmeye hazır olun. Hepinizi üteceğim.", ConsoleColor.DarkGreen);

        deck = DeckHelper.CreateFullDeck(8);
        DeckHelper.ShuffleDecks(deck);
        CreateAFulTable();
    }

    public void PlayRounds(int roundNumber = 1)
    {
        for (int i = 0; i < roundNumber; i++)
        {
            StartNewRound();
            AskAllPlayersForActions();
            PlayForDealer();
            CheckHandsAndDeliverPrizes();
            CheckForHusoka();
            CollectAllCards();
        }
        ReportEarnings();
    }



    private void CheckForHusoka()
    {
        if (!husokaIsMorting) return;
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

    private void CheckHandsAndDeliverPrizes()
    {
        Hand playerHand;
        foreach (var player in players.Where(x => x.HasBetted))
        {
            decimal earning = 0;
            playerHand = player.Hand;

            //TODO-HUS bir oyuncu patladığında, kasa da patlarsa beraberlik mi olur? Kasa normalde çekmez kazanır. Ama çoklu oyuncu olunca?
            //player loses
            if (playerHand.IsBusted)
            {
                ConsoleHelper.WriteLine($"Unfortunately {player.Name} has lost the round", ConsoleColor.DarkRed);
                player.LosingStreak++;
                player.NotWinningStreak++;
                player.WinningStreak = 0;
            }

            //player has blackjack
            else if (playerHand.IsBlackJack && !dealer.Hand.IsBlackJack)
            {
                earning = playerHand.BetAmount * 2.5M;
                PayBackToPlayer(player, earning);
                ConsoleHelper.WriteLine($"Yaaay!! It is a blackjack!!! {player.Name} has won {earning} TL", ConsoleColor.Green);
                player.LosingStreak = 0;
                player.NotWinningStreak = 0;
                player.WinningStreak++;
            }

            //player wins
            else if (dealer.Hand.IsBusted || playerHand.HandValue > dealer.Hand.HandValue)
            {
                earning = playerHand.BetAmount * 2;
                PayBackToPlayer(player, earning);
                ConsoleHelper.WriteLine($"Yess!! {player.Name} has won the round and won {earning} TL", ConsoleColor.Green);
                player.LosingStreak = 0;
                player.NotWinningStreak = 0;
                player.WinningStreak++;
            }

            //it is a push
            else if (dealer.Hand.HandValue == playerHand.HandValue)
            {
                earning = playerHand.BetAmount;
                PayBackToPlayer(player, earning);
                ConsoleHelper.WriteLine($"It's a push!! {player.Name} has got {earning} TL back", ConsoleColor.DarkYellow);
                player.LosingStreak = 0;
                player.NotWinningStreak++;
                player.WinningStreak = 0;
            }

            //player loses
            else if (dealer.Hand.HandValue > playerHand.HandValue)
            {
                ConsoleHelper.WriteLine($"Nooo!! {player.Name} has lost the round", ConsoleColor.DarkRed);
                player.LosingStreak++;
                player.NotWinningStreak++;
                player.WinningStreak = 0;
            }
            else
            {
                throw new Exception("Bu durumu incelemeliyiz.");
            }

            ConsoleHelper.WriteLine($"{player.Name}'s current balance: {player.Balance}", ConsoleColor.Blue);
            ConsoleHelper.WriteLine($"House's current balance: {dealer.Balance}", ConsoleColor.Magenta);
        }
    }

    private void PayBackToPlayer(Player player, decimal amount)
    {
        player.Balance += amount;
        dealer.Balance -= amount;

        if(player == husokaBettedFor)
        {
            //TODO-HUS bet behind yaptığımız split yaparsa nasıl kazanıyorz?
            var husoEarning = player.Hand.IsBlackJack ? currentHusokaBet * 2.5M : currentHusokaBet * 2;
            husoka.Balance += husoEarning;
            ConsoleHelper.WriteLine($"Heeeellll yeaaah!!!!! {husoka.Name} has won {husoEarning} TL. {husoka.Name}'s current balance: {husoka.Balance}", ConsoleColor.DarkCyan);
            husokaIsMorting = false;
            husokaBettedFor = null;
            currentHusokaBet = 0;
            Task.Delay(500);
        }
    }

    private void PlayForDealer()
    {
        //If everyone is busted, dealer wins. 
        if (!players.Any(x => !x.Hand.IsBusted)
            && !players.Any(x => x.SplittedHand is not null && !x.SplittedHand.IsBusted)) return;

        ConsoleHelper.WriteLine($"{dealer.Name}'s second card is: {dealer.Hand.Cards[1].CardType} - {dealer.Hand.Cards[1].CardValue}", ConsoleColor.Magenta);
        ConsoleHelper.WriteLine($"{dealer.Name}'s current hand: {dealer.Hand.HandValue}", ConsoleColor.Magenta);

        //Otherwise dealers opens card and hits until at least 17
        while (dealer.Hand.HandValue < 17) {
            DealCard(dealer.Hand);
            ConsoleHelper.WriteLine($"{dealer.Name} hits. Now the hand is : {dealer.Hand.HandValue}");
        } 
    }

    private void AskAllPlayersForActions()
    {
        foreach (var player in players.Where(x => x.HasBetted))
        {
            var shouldAskForNormalHand = true;
            while (shouldAskForNormalHand)
            {
                var playerAction = AskForAction(player.Hand);
                shouldAskForNormalHand = ApplyPlayerAction(player, player.Hand, playerAction);
            }
            var shouldAskForSplitHand = true;
            while (shouldAskForSplitHand)
            {
                if (player.SplittedHand is null) break;
                if (player.SplittedHand.Cards.Count == 1) DealCard(player.SplittedHand);
                var playerAction = AskForAction(player.SplittedHand, true);
                shouldAskForSplitHand = ApplyPlayerAction(player, player.SplittedHand, playerAction);
            }

            if (player.Hand.HandValue > 21) player.Hand.IsBusted = true;
        }
    }

    private bool ApplyPlayerAction(Player player, Hand hand, CardAction playerAction)
    {
        return playerAction switch
        {
            CardAction.Stand => ApplyStand(player, hand),
            CardAction.Hit => ApplyHit(player, hand),
            CardAction.Double => ApplyDouble(player, hand),
            CardAction.Split => ApplySplit(player),
            _ => throw new Exception("La nasıl olur bu!!!")
        };
    }

    private bool ApplyStand(Player player, Hand hand)
    {
        ConsoleHelper.WriteLine($"{player.Name} stands. Now the hand is : {hand.HandValue}", ConsoleColor.DarkYellow);
        return false;
    }

    private bool ApplySplit(Player player)
    {
        player.SplittedHand = new Hand();
        var splitCard = player.Hand.Cards[1];
        player.Hand.Cards.Remove(splitCard);
        player.SplittedHand.Cards.Add(splitCard);
        
        player.SplittedHand.HandValue = CardManager.GetCountOfHand(player.SplittedHand);
        player.Hand.HandValue = CardManager.GetCountOfHand(player.Hand);
        ConsoleHelper.WriteLine($"{player.Name} splits the cards. Now the first hand is : {player.Hand.HandValue} and the second hand is : {player.SplittedHand.HandValue}", ConsoleColor.Black, ConsoleColor.Cyan);
        DealCard(player.Hand);
        ConsoleHelper.WriteLine($"{player.Name}'s new card is {player.Hand.Cards[1].CardValue}. Now the first hand is : {player.Hand.HandValue}", ConsoleColor.Black, ConsoleColor.Cyan);
        return true;
    }

    private bool ApplyHit(Player player, Hand hand)
    {
        DealCard(hand);
        ConsoleHelper.WriteLine($"{player.Name} hits. Now the hand is : {hand.HandValue}", ConsoleColor.DarkCyan);
        return hand.HandValue<21;
    }

    private bool ApplyDouble(Player player, Hand hand)
    {
        DealCard(hand);
        player.Balance -= hand.BetAmount;
        dealer.Balance += hand.BetAmount;
        hand.BetAmount *= 2;
        
        ConsoleHelper.WriteLine($"{player.Name} doubles. Now the hand is : {hand.HandValue}", ConsoleColor.Red);
        return false;
    }

    private CardAction AskForAction(Hand? hand, bool isSplitHand = false)
    {
        if (hand is null) return CardAction.Stand;
        return OptimalMoveManager.MakeOptimalMove(dealer.Hand.Cards[0], hand, isSplitHand);
    }

    private void StartNewRound()
    {
        roundNo++;
        ConsoleHelper.WriteLine($"--------------------------- ROUND - {roundNo} HAS STARTED ---------------------------", ConsoleColor.Magenta);
        Console.WriteLine();
        AcceptTheBets();
        DealTheCards();
        WriteTableCardsForRound();
    }

    private void AcceptTheBets()
    {
        foreach (var player in players.Where(x => x.Spot > 0))
        {
            player.HasBetted = true;
            player.Balance -= 100;
            player.Hand.BetAmount = 100;
            dealer.Balance += 100;
        }

        if(!husokaIsMorting && players.Any(x => x.NotWinningStreak >= 5) )
        {
            husokaIsMorting = true;
            if (currentHusokaBet == 0) currentHusokaBet = 10;
            else currentHusokaBet *= 2;
            husoka.Balance -= currentHusokaBet;
            dealer.Balance -= currentHusokaBet;
            husoka.HasBetted = true;
            husokaBettedFor = players.First(x => x.NotWinningStreak >= 5);
            ConsoleHelper.WriteLine($"{husoka.Name}'s morting. Let's gooo!. Our balance is : {husoka.Balance}", ConsoleColor.Black, ConsoleColor.Cyan);
        }

        else if (husokaIsMorting)
        {
            currentHusokaBet *= 2;
            husoka.Balance -= currentHusokaBet;
            dealer.Balance -= currentHusokaBet;
            husoka.HasBetted = true;
            ConsoleHelper.WriteLine($"{husoka.Name}'s mooorting. We bet another {currentHusokaBet} TL. Our balance is : {husoka.Balance}", ConsoleColor.Black, ConsoleColor.Cyan);
        }
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

        if(card.CardType == CardType.ShufflerCard)
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

    private int GivePlayerSpot(int spotNo = 1)
    {
        if (spotNo == 1) spotNo = Array.IndexOf(spots, false);
        if (spotNo == -1) return -1;

        spots[spotNo] = true;
        return spotNo;
    }

    public void WriteAllCards()
    {
        var counter = 0;
        foreach (var card in deck)
        {
            counter++;
            WriteCard(card);
        }
        Console.WriteLine(counter);
    }

    public static void WriteCard(Card card)
    {
        ConsoleColor color = card.CardType switch
        {
            CardType.Spades or CardType.Clubs => ConsoleColor.DarkGray,
            CardType.Hearts or CardType.Diamonds => ConsoleColor.Red,
            CardType.ShufflerCard => ConsoleColor.Cyan,
            _ => throw new NotImplementedException("Unhandled CardType"),
        };

        ConsoleHelper.Write(card.CardValue.ToString(), color);
        ConsoleHelper.Write(" - ");
        ConsoleHelper.Write(card.CardType.ToString(), color);
        Console.WriteLine();
    }

    public void WriteTableCardsForRound()
    {
        ConsoleHelper.WriteLine("Dealer has :", ConsoleColor.DarkCyan);
        WriteCard(dealer.Hand.Cards[0]);

        foreach (var player in players.Where(x => x.HasBetted))
        {
            ConsoleHelper.WriteLine(player.Name + " has :", ConsoleColor.Blue);
            WriteCard(player.Hand.Cards[0]);
            WriteCard(player.Hand.Cards[1]);
        }
    }

    private void ReportEarnings()
    {
        ConsoleHelper.WriteLine("-----------------------------------------------------------------------------------------", ConsoleColor.DarkCyan);
        foreach (var player in players)
        {
            ConsoleHelper.WriteLine($"{player.Name} current balance is : {player.Balance}", ConsoleColor.Blue);
        }
        ConsoleHelper.WriteLine($"{husoka.Name} current balance is : {husoka.Balance}", ConsoleColor.DarkBlue);
        ConsoleHelper.WriteLine($"{dealer.Name} current balance is : {dealer.Balance}", ConsoleColor.DarkCyan);
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