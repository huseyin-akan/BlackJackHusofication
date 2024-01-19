﻿using BlackJackHusofication.Helpers;
using BlackJackHusofication.Models;

namespace BlackJackHusofication.Managers;

internal class BalanceManager
{
    public static void CheckHandsAndDeliverPrizes(IEnumerable<Player> players, Dealer dealer, Husoka husoka)
    {
        Hand playerHand;
        foreach (var player in players)
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
                PayBackToPlayer(player, dealer, earning, husoka);
                ConsoleHelper.WriteLine($"Yaaay!! It is a blackjack!!! {player.Name} has won {earning} TL", ConsoleColor.Green);
                player.LosingStreak = 0;
                player.NotWinningStreak = 0;
                player.WinningStreak++;
            }

            //player wins
            else if (dealer.Hand.IsBusted || playerHand.HandValue > dealer.Hand.HandValue)
            {
                earning = playerHand.BetAmount * 2;
                PayBackToPlayer(player, dealer, earning, husoka);
                ConsoleHelper.WriteLine($"Yess!! {player.Name} has won the round and won {earning} TL", ConsoleColor.Green);
                player.LosingStreak = 0;
                player.NotWinningStreak = 0;
                player.WinningStreak++;
            }

            //it is a push
            else if (dealer.Hand.HandValue == playerHand.HandValue)
            {
                earning = playerHand.BetAmount;
                PayBackToPlayer(player, dealer, earning, husoka);
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
    private static void PayBackToPlayer(Player player, Dealer dealer, decimal amount, Husoka husoka)
    {
        player.Balance += amount;
        dealer.Balance -= amount;

        if (player == husoka.HusokaBettedFor)
        {
            //TODO-HUS bet behind yaptığımız split yaparsa nasıl kazanıyorz?
            var husoEarning = player.Hand.IsBlackJack ? husoka.CurrentHusokaBet * 2.5M : husoka.CurrentHusokaBet * 2;
            husoka.Balance += husoEarning;
            ConsoleHelper.WriteLine($"Heeeellll yeaaah!!!!! {husoka.Name} has won {husoEarning} TL. {husoka.Name}'s current balance: {husoka.Balance}", ConsoleColor.DarkCyan);
            husoka.HusokaIsMorting = false;
            husoka.HusokaBettedFor = null;
            husoka.CurrentHusokaBet = 0;
        }
    }
}
