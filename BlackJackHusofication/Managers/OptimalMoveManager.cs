using BlackJackHusofication.Models;

namespace BlackJackHusofication.Managers;

internal class OptimalMoveManager
{
    public static CardAction MakeOptimalMove(Card dealerCard, Hand playerHand, bool isSplitHand)
    {
        var dealersCardValue = CardManager.GetCardCount(dealerCard);
        var playerHandIsPair = playerHand.Cards.Count == 2 && playerHand.Cards[0].CardValue == playerHand.Cards[1].CardValue;
        var playerFirstCard = playerHand.Cards[0];
        var firstCardValue = CardManager.GetCardCount(playerHand.Cards[0]);

        //When player has pairs:
        if (playerHandIsPair)
        {
            if (playerFirstCard.CardValue == CardValue.Ace || firstCardValue == 8) return CardAction.Split;
            if (firstCardValue == 10) return CardAction.Stand;
            if (firstCardValue == 9)
                if (dealersCardValue == 7 || dealersCardValue == 10 || dealerCard.CardValue == CardValue.Ace) return CardAction.Stand;
                else return CardAction.Split;
            if(firstCardValue == 7)
                if(dealersCardValue>=2 || dealersCardValue <= 7)
        }


        if (dealersCardValue == 2)
        {
            if (playerHand.HandValue >= 13) return CardAction.Stand;
            if (playerHand.HandValue == 12) return CardAction.Hit;
            if (playerHand.HandValue == 10 || playerHand.HandValue == 11) return CardAction.Double;
            if (playerHand.HandValue >= 5 && playerHand.HandValue <= 9) return CardAction.Hit;
        }



        return CardAction.Stand;
    }
}