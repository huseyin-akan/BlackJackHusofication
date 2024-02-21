using BlackJackHusofication.Model.Exceptions;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

internal class BalanceManager
{
    public static decimal CheckHandsAndDeliverPrizes(Table table, int spotId)
    {
        var spot = table.Spots.FirstOrDefault(s => s.Id == spotId) ?? throw new Exception("Hata var aga");
        
        Hand currentHand;
        decimal totalEarning = 0;

        for (int i = 0; i < 2; i++)
        {
            if (spot.Player is null) continue;

            if (i == 0) currentHand = spot.Hand;
            else if (spot.SplittedHand is null) continue;
            else currentHand = spot.SplittedHand;

            decimal earning = 0;

            //TODO-HUS bir oyuncu patladığında, kasa da patlarsa beraberlik mi olur? Kasa normalde çekmez kazanır. Ama çoklu oyuncu olunca?
            //player loses
            if (currentHand.IsBusted)
            {
                spot.Player.LosingStreak++;
                spot.Player.NotWinningStreak++;
                spot.Player.WinningStreak = 0;
            }

            //player has blackjack
            else if (currentHand.IsBlackJack && !table.Dealer.Hand.IsBlackJack)
            {
                earning = spot.BetAmount * 2.5M;
                PayToPlayer(spot.Player, table, earning);
                spot.Player.LosingStreak = 0;
                spot.Player.NotWinningStreak = 0;
                spot.Player.WinningStreak++;
            }

            //player wins
            else if (table.Dealer.Hand.IsBusted || currentHand.HandValue > table.Dealer.Hand.HandValue)
            {
                earning = spot.BetAmount * 2;
                PayToPlayer(spot.Player, table, earning);
                spot.Player.LosingStreak = 0;
                spot.Player.NotWinningStreak = 0;
                spot.Player.WinningStreak++;
            }

            //dealer has blackjack player loses
            else if (table.Dealer.Hand.IsBlackJack)
            {
                spot.Player.LosingStreak++;
                spot.Player.NotWinningStreak++;
                spot.Player.WinningStreak = 0;
            }

            //it is a push
            else if (table.Dealer.Hand.HandValue == currentHand.HandValue)
            {
                earning = spot.BetAmount;
                PayToPlayer(spot.Player, table, earning);
                spot.Player.LosingStreak = 0;
                spot.Player.NotWinningStreak++;
                spot.Player.WinningStreak = 0;
            }

            //player loses
            else if (table.Dealer.Hand.HandValue > currentHand.HandValue)
            {
                spot.Player.LosingStreak++;
                spot.Player.NotWinningStreak++;
                spot.Player.WinningStreak = 0;
            }
            else
            {
                throw new Exception("Bu durumu incelemeliyiz.");
            }

            totalEarning += earning;
        }

        return totalEarning;
    }

    private static void PayToPlayer(Player player, Table table, decimal amount)
    {
        player.Balance += amount;
        table.Balance -= amount;
    }

    public static void PlayerBet(Player player, BjGame game, decimal betAmount, int spotIndex)
    {
        player.Balance -= betAmount;
        game.Table.Balance += betAmount;
        var spot = game.Table.Spots.FirstOrDefault(x => x.Id == spotIndex) 
            ?? throw new BjGameException("Oturmak isteği attığınız koltuk mevcut değil!!!");

        spot.BetAmount = betAmount;
    }

    public static void PlayerDouble(Spot spot, Table table)
    {
        ArgumentNullException.ThrowIfNull(spot.Player);

        spot.Player.Balance -= spot.BetAmount;
        table.Balance += spot.BetAmount;
        spot.BetAmount *= 2;
    }

    public static void PlayerSplit(Spot spot, Table table)
    {
        ArgumentNullException.ThrowIfNull(spot.Player);

        spot.Player.Balance -= spot.BetAmount;
        table.Balance += spot.BetAmount;
        //We dont increase the bet amount here, because we will calculate each hand alone and according to current bet amount
    }
}