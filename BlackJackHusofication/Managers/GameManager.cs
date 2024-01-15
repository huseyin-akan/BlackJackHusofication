using BlackJackHusofication.Helpers;
using BlackJackHusofication.Models;

namespace BlackJackHusofication.Managers;

internal class GameManager
{
    private int roundNo = 0;
    private List<Card> deck = [];
    private readonly List<Card> playedCard = [];
    private Dealer dealer;
    private readonly List<Player> players = [];
    private readonly bool[] spots = new bool[8];

    public void StartNewGame()
    {
        roundNo = 0;
        dealer = new Dealer() { Id = 1, Name = "Dealer", Balance = 0, Spot = GivePlayerSpot(0) };

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
            && !players.Any(x => x.DoubleHand is not null && !x.DoubleHand.IsBusted)) return; 
        
        //Otherwise dealers opens card
    }

    private void AskAllPlayersForActions()
    {
        foreach (var player in players.Where(x => x.HasBetted))
        {
            var shouldAskPlayer = true;
            while (shouldAskPlayer)
            {
                var playerAction = AskForAction(player);
                shouldAskPlayer = ApplyPlayerAction(player, playerAction);
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
            _ => throw new Exception("La nasıl olur bu!!!")
        };
    }

    private bool ApplyHit(Player player)
    {
        throw new NotImplementedException();
    }

    private bool ApplyDouble(Player player)
    {
        throw new NotImplementedException();
    }

    private CardAction AskForAction(Player player)
    {
        return OptimalMoveManager.MakeOptimalMove(dealer.Hand.Cards[0], player.Hand);
    }

    public void StartNewRound()
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

    public void DealTheCards()
    {
        for (int i = 0; i < 2; i++)
        {
            //Deal for all players
            foreach (var player in players.Where(x => x.HasBetted)) DealCard(player);
 
            //Deal for dealer
            DealCard(dealer);
        }
    }

    public void DealCard(Player player)
    {
        var card = deck[0];
        deck.Remove(deck[0]);
        player.Hand.Cards.Add(card);
    }

    public int GivePlayerSpot(int spotNo = 1)
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