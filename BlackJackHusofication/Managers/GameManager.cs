using BlackJackHusofication.Helpers;
using BlackJackHusofication.Models;

namespace BlackJackHusofication.Managers;

internal class GameManager
{
    private int roundNo;
    private List<Card> deck;
    private readonly List<Card> playedCards;
    private readonly Dealer dealer;
    private readonly List<Player> players;
    private readonly bool[] spots; 

    public GameManager()
    {
        roundNo = 0;
        spots = new bool[8];
        players = [];
        playedCards = [];
        deck = [];
        dealer = new Dealer() { Id = 1, Name = "Dealer", Balance = 0, Spot = GivePlayerSpot(0) };
        
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
            CalculateEarnings();
            ReportTheRound();
        }
    }

    private void ReportTheRound()
    {
        throw new NotImplementedException();
    }

    private void CalculateEarnings()
    {
        throw new NotImplementedException();
    }

    private void PlayForDealer()
    {
        //If everyone is busted, dealer wins. 
        if (!players.Any(x => !x.Hand.IsBusted) 
            && !players.Any(x => x.SplittedHand is not null && !x.SplittedHand.IsBusted)) return;

        //Otherwise dealers opens card and hits until at least 17
        while(dealer.Hand.HandValue < 17) DealCard(dealer.Hand);
    }

    private void AskAllPlayersForActions()
    {
        foreach (var player in players.Where(x => x.HasBetted))
        {
            var shouldAskForNormalHand = true;
            while (shouldAskForNormalHand)
            {
                var playerAction = AskForAction(player.Hand);
                shouldAskForNormalHand = ApplyPlayerAction(player, playerAction);
            }
            var shouldAskForSplitHand = true;
            while (shouldAskForSplitHand)
            {
                var playerAction = AskForAction(player.SplittedHand, true);
                shouldAskForNormalHand = ApplyPlayerAction(player, playerAction);
            }
        }
    }

    private bool ApplyPlayerAction(Player player, CardAction playerAction)
    {
        return playerAction switch
        {
            CardAction.Stand => false,
            CardAction.Hit => ApplyHit(player),
            CardAction.Double => ApplyDouble(player),
            CardAction.Split => ApplySplit(player),
            _ => throw new Exception("La nasıl olur bu!!!")
        };
    }

    private bool ApplySplit(Player player)
    {
        player.SplittedHand = new Hand();
        var splitCard = player.Hand.Cards[1];
        player.Hand.Cards.Remove(splitCard);
        player.SplittedHand.Cards.Add(splitCard);
        DealCard(player.Hand);
        return true;
    }

    private bool ApplyHit(Player player)
    {
        throw new NotImplementedException();
    }

    private bool ApplyDouble(Player player)
    {
        throw new NotImplementedException();
    }

    private CardAction AskForAction(Hand? hand, bool isSplitHand= false)
    {
        if (hand is null) return CardAction.Stand;
        return OptimalMoveManager.MakeOptimalMove(dealer.Hand.Cards[0], hand, isSplitHand);
    }

    private void StartNewRound()
    {
        roundNo++;
        AcceptTheBets();
        DealTheCards();
        WriteTableCardsForRound();
    }

    private void AcceptTheBets()
    {
        foreach (var player in players.Where(x => x.Spot > 0))
        {
            player.HasBetted = true;
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

        players.Add(new Player() { Id = 8, Name = "Husoka", Balance = 1_270 });
    }

    private void DealTheCards()
    {
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
        hand.Cards.Add(card);
        hand.HandValue = CardManager.GetCountOfHand(hand);
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