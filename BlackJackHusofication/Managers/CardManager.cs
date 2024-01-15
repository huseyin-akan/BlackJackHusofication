using BlackJackHusofication.Models;

namespace BlackJackHusofication.Managers;

internal class CardManager
{
    public static int GetCountOfHand(Hand hand)
    {
        return 0;
    }

    public static int GetCardCount(Card card)
    {
        return card.CardValue switch
        {
            CardValue.Two => 2,
            _ => throw new Exception("Böyle bir card değeri yok.")
        };
    }
}
