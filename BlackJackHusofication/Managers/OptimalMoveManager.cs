using BlackJackHusofication.Models;

namespace BlackJackHusofication.Managers;

internal class OptimalMoveManager
{
    public static CardAction MakeOptimalMove(Card dealersCard, Hand playersHand)
    {
        int playerHandCount = 10; //TODO-HUS
        int dealerCardCount = 10; //TODO-HUS
        if(dealerCardCount == 6)
        {
            if (playerHandCount == 20) return CardAction.Stand;
        }
        return CardAction.Hit;
    }
}
