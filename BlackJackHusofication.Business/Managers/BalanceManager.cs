using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Model.Exceptions;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

internal class BalanceManager
{
    public static void CheckHandsAndDeliverPrizes(IEnumerable<Player> players, Dealer dealer, Husoka husoka)
    {
        //Hand playerHand;
        //foreach (var player in players)
        //{
        //    decimal earning = 0;
        //    playerHand = player.Hand;

        //    //TODO-HUS bir oyuncu patladığında, kasa da patlarsa beraberlik mi olur? Kasa normalde çekmez kazanır. Ama çoklu oyuncu olunca?
        //    //player loses
        //    if (playerHand.IsBusted)
        //    {
        //        LogHelper.WriteLine($"Unfortunately {player.Name} has lost the round", ConsoleColor.DarkRed);
        //        player.LosingStreak++;
        //        player.NotWinningStreak++;
        //        player.WinningStreak = 0;
        //    }

        //    //player has blackjack
        //    else if (playerHand.IsBlackJack && !dealer.Hand.IsBlackJack)
        //    {
        //        earning = playerHand.BetAmount * 2.5M;
        //        PayBackToPlayer(player, dealer, earning, husoka);
        //        LogHelper.WriteLine($"Yaaay!! It is a blackjack!!! {player.Name} has won {earning} TL", ConsoleColor.Green);
        //        player.LosingStreak = 0;
        //        player.NotWinningStreak = 0;
        //        player.WinningStreak++;
        //    }

        //    //player wins
        //    else if (dealer.Hand.IsBusted || playerHand.HandValue > dealer.Hand.HandValue)
        //    {
        //        earning = playerHand.BetAmount * 2;
        //        PayBackToPlayer(player, dealer, earning, husoka);
        //        LogHelper.WriteLine($"Yess!! {player.Name} has won the round and won {earning} TL", ConsoleColor.Green);
        //        player.LosingStreak = 0;
        //        player.NotWinningStreak = 0;
        //        player.WinningStreak++;
        //    }

        //    //dealer has blackjack player loses
        //    else if (dealer.Hand.IsBlackJack)
        //    {
        //        LogHelper.WriteLine($"Fudgeee!! {player.Name} has lost the round", ConsoleColor.DarkRed);
        //        player.LosingStreak++;
        //        player.NotWinningStreak++;
        //        player.WinningStreak = 0;
        //    }

        //    //it is a push
        //    else if (dealer.Hand.HandValue == playerHand.HandValue)
        //    {
        //        earning = playerHand.BetAmount;
        //        PayBackToPlayer(player, dealer, earning, husoka);
        //        LogHelper.WriteLine($"It's a push!! {player.Name} has got {earning} TL back", ConsoleColor.DarkYellow);
        //        player.LosingStreak = 0;
        //        player.NotWinningStreak++;
        //        player.WinningStreak = 0;
        //    }

        //    //player loses
        //    else if (dealer.Hand.HandValue > playerHand.HandValue)
        //    {
        //        LogHelper.WriteLine($"Nooo!! {player.Name} has lost the round", ConsoleColor.DarkRed);
        //        player.LosingStreak++;
        //        player.NotWinningStreak++;
        //        player.WinningStreak = 0;
        //    }
        //    else
        //    {
        //        throw new Exception("Bu durumu incelemeliyiz.");
        //    }

        //    LogHelper.WriteLine($"{player.Name}'s current balance: {player.Balance}", ConsoleColor.Blue);
        //    LogHelper.WriteLine($"House's current balance: {dealer.Balance}", ConsoleColor.Magenta);
        //}
    }

    private static void PayBackToPlayer(Player player, Dealer dealer, decimal amount, Husoka husoka)
    {
        //player.Balance += amount;
        //dealer.Balance -= amount;

        //if (player == husoka.HusokaBettedFor)
        //{
        //    //TODO-HUS bet behind yaptığımız split yaparsa nasıl kazanıyorz?
        //    var husoEarning = player.Hand.IsBlackJack ? husoka.CurrentHusokaBet * 2.5M : husoka.CurrentHusokaBet * 2;
        //    husoka.Balance += husoEarning;
        //    LogHelper.WriteLine($"Heeeellll yeaaah!!!!! {husoka.Name} has won {husoEarning} TL. {husoka.Name}'s current balance: {husoka.Balance}", ConsoleColor.DarkCyan);
        //    husoka.HusokaIsMorting = false;
        //    husoka.HusokaBettedFor = null;
        //    husoka.CurrentHusokaBet = 0;
        //}
    }

    public static void PlayerBet(Player player, BjGame game, decimal betAmount, int spotIndex)
    {
        player.Balance -= betAmount;
        game.Table.Balance += betAmount;
        var spot = game.Table.Spots.FirstOrDefault(x => x.Id == spotIndex) 
            ?? throw new BjGameException("Oturmak isteği attığınız koltuk mevcut değil!!!");

        spot.BetAmount = betAmount;
    }
}
