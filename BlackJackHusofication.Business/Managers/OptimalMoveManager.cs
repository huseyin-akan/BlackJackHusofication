using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Managers;

public class OptimalMoveManager
{
    public static CardAction MakeOptimalMove(Card dealerCard, Hand playerHand, bool isSplitHand = false)
    {
        var dealersCardValue = CardManager.GetCardCount(dealerCard);
        var playerHandIsPair = playerHand.Cards.Count == 2 && playerHand.Cards[0].CardValue == playerHand.Cards[1].CardValue;
        var playerFirstCard = playerHand.Cards[0];
        var firstCardValue = CardManager.GetCardCount(playerHand.Cards[0]);

        //When player has pairs:
        if (playerHandIsPair && !isSplitHand)
        {
            if (playerFirstCard.CardValue == CardValue.Ace) return CardAction.Split;
            if (firstCardValue == 10) return CardAction.Stand;
            if (firstCardValue == 9)
                if (dealersCardValue == 7 || dealersCardValue == 10 || dealerCard.CardValue == CardValue.Ace) return CardAction.Stand;
                else return CardAction.Split;
            if (firstCardValue == 8)
                if (dealersCardValue <= 9) return CardAction.Split;
                else return CardAction.Hit;
            if (firstCardValue == 7)
                if (dealersCardValue >= 2 && dealersCardValue <= 7) return CardAction.Split;
                else return CardAction.Hit;
            if (firstCardValue == 6)
                if (dealersCardValue >= 2 && dealersCardValue <= 6) return CardAction.Split;
                else return CardAction.Hit;
            if (firstCardValue == 5)
                if (dealersCardValue >= 2 && dealersCardValue <= 9) return CardAction.Double;
                else return CardAction.Hit;
            if (firstCardValue == 4)
                if (dealersCardValue == 5 || dealersCardValue == 6) return CardAction.Split;
                else return CardAction.Hit;
            if (firstCardValue <= 3)
                if (dealersCardValue >= 2 && dealersCardValue <= 7) return CardAction.Split;
                else return CardAction.Hit;
        }

        //When player has between 4 - 11 handvalue
        if (playerHand.HandValue <= 8) return CardAction.Hit;
        if (playerHand.HandValue == 9)
            if (dealersCardValue >= 3 && dealersCardValue <= 6)
                if (playerHand.Cards.Count == 2) return CardAction.Double;
                else return CardAction.Hit;
            else return CardAction.Hit;
        if (playerHand.HandValue == 10)
            if (dealersCardValue <= 9)
                if (playerHand.Cards.Count == 2) return CardAction.Double;
                else return CardAction.Hit;
            else return CardAction.Hit;
        if (playerHand.HandValue == 11)
            if (dealerCard.CardValue == CardValue.Ace) return CardAction.Hit;
            else if (playerHand.Cards.Count == 2) return CardAction.Double;
            else return CardAction.Hit;


        //When player has a bigger handvalue than soft 11
        if (playerHand.IsSoft)
        {
            if (playerHand.HandValue >= 12 && playerHand.HandValue <= 14) //TODO-HUS soft-12'yi 13'e uydurduk. Başka kaynağa da bakalım.
                if (dealersCardValue >= 5 && dealersCardValue <= 6)
                    if (playerHand.Cards.Count == 2) return CardAction.Double;
                    else return CardAction.Hit;
                else return CardAction.Hit;
            if (playerHand.HandValue >= 15 && playerHand.HandValue <= 16)
                if (dealersCardValue >= 4 && dealersCardValue <= 6)
                    if (playerHand.Cards.Count == 2) return CardAction.Double;
                    else return CardAction.Hit;
                else return CardAction.Hit;
            if (playerHand.HandValue == 17)
                if (dealersCardValue >= 3 && dealersCardValue <= 6)
                    if (playerHand.Cards.Count == 2) return CardAction.Double;
                    else return CardAction.Hit;
                else if (dealersCardValue == 2 || dealersCardValue == 7 || dealersCardValue == 8) return CardAction.Stand;
                else return CardAction.Hit;
            if (playerHand.HandValue >= 19 && playerHand.HandValue <= 20) return CardAction.Stand;
        }

        //When player has a bigger handvalue than 11
        if (playerHand.HandValue == 12)
            if (dealersCardValue >= 4 && dealersCardValue <= 6) return CardAction.Stand;
            else return CardAction.Hit;
        if (playerHand.HandValue >= 13 && playerHand.HandValue <= 16)
            if (dealersCardValue >= 2 && dealersCardValue <= 6) return CardAction.Stand;
            else return CardAction.Hit;
        if (playerHand.HandValue >= 17 && playerHand.HandValue <= 21) return CardAction.Stand;

        throw new Exception("Kod buraya kadar nasıl geldi dayı!");
    }
}