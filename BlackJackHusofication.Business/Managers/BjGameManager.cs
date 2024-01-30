using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;
using Newtonsoft.Json.Linq;

namespace BlackJackHusofication.Business.Managers;

public class BjGameManager : IGameManager
{
    static readonly List<BjRoom> rooms = [];

    public static void CreateNewRoom(string roomName, int roomId)
    {
        BjRoom room = new() { Name = roomName, RoomId = roomId };
        room.Table.Deck = DeckHelper.CreateFullDeck(6);
        DeckHelper.ShuffleDecks(room.Table.Deck);
        rooms.Add(room);
    }

    public async static Task StartGame(BjRoom room)
    {
        bool exitGame = false;
        while (exitGame)
        {
            var startNewRound = await AcceptTheBets(room);
            if (!startNewRound) continue;

            room.RoundNo++;
            DealTheCards(room);

            await AskAllPlayersForActions(room);
        }
    }

    private async static Task<bool> AcceptTheBets(BjRoom room)
    {
        room.IsAcceptingBets = true;
        await Task.Delay(30_000);
        room.IsAcceptingBets = false;

        return room.Table.Players.Any(x => x.HasBetted);
    }

    public static void PlayerBet(Player player, BjRoom room, decimal betAmount)
    {
        player.HasBetted = true;
        player.Balance -= betAmount;
        player.Hand.BetAmount = betAmount;
        room.Table.Balance += betAmount;
    }

    private static void DealTheCards(BjRoom room)
    {
        if (room.Table.IsShoeShouldChange)
        {
            List<Card> collectedShoe = [.. room.Table.PlayedCards, .. room.Table.Deck, room.Table.ShufflerCard!];
            room.Table.Deck = DeckHelper.ShuffleDecks(collectedShoe);
            room.Table.PlayedCards = [];
            room.Table.ShufflerCard = null;
            room.Table.IsShoeShouldChange = false;
        }

        for (int i = 0; i < 2; i++)
        {
            //Deal for all players who has betted
            foreach (var player in room.Table.Players.Where(x => x.HasBetted)) DealCard(room, player.Hand);

            //Deal for dealer
            DealCard(room, room.Table.Dealer.Hand);
        }
    }

    private static void DealCard(BjRoom room, Hand hand)
    {
        var card = room.Table.Deck[0];
        room.Table.Deck.Remove(card);

        if (card.CardType == CardType.ShufflerCard)
        {
            room.Table.IsShoeShouldChange = true;
            room.Table.ShufflerCard = card with { };
            card = room.Table.Deck[0];
            room.Table.Deck.Remove(card);
        }

        hand.Cards.Add(card);
        hand.HandValue = CardManager.GetCountOfHand(hand);

        if (hand.HandValue > 21) hand.IsBusted = true;
        else if (hand.HandValue == 21 && hand.Cards.Count == 2) hand.IsBlackJack = true;
    }

    private static async Task AskAllPlayersForActions(BjRoom room)
    {
        foreach (var player in room.Table.Players.Where(x => x.HasBetted))
        {
            var shouldAskForNormalHand = true;
            //while (shouldAskForNormalHand)
            //{
            //    try
            //    {
            //        // Wait for 30 seconds with the ability to cancel
            //        await Task.Delay(TimeSpan.FromSeconds(30), room._cancellationTokenSource.Token);

            //        // Handle timeout (default to stand) 
            //        // play stand here
            //        // await Clients.All.PlayerAction(CardAction.Stand);
            //    }
            //    catch (TaskCanceledException)
            //    {
            //        // Timeout was canceled due to player action
            //    }

            //    //Apply player action here and maybe ask player for new action again.

            //    var playerAction = AskForAction(player.Hand);
            //    shouldAskForNormalHand = await ApplyPlayerAction(player, player.Hand, playerAction);
            //}

            //var shouldAskForSplitHand = true;
            //while (shouldAskForSplitHand)
            //{
            //    if (player.SplittedHand is null) break;
            //    if (player.SplittedHand.Cards.Count == 1) DealCard(player.SplittedHand);
            //    var playerAction = AskForAction(player.SplittedHand, true);
            //    shouldAskForSplitHand = await ApplyPlayerAction(player, player.SplittedHand, playerAction);
            //}

            if (player.Hand.HandValue > 21) player.Hand.IsBusted = true;
        }
    }

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
    //    return OptimalMoveManager.MakeOptimalMove(_simulation.Dealer.Hand.Cards[0], hand, isSplitHand);
    //}
}