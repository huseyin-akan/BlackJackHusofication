using BlackJackHusofication.Models;

namespace BlackJackHusofication.Managers;

public class CardManager
{
    public static int GetCountOfHand(Hand hand)
    {
        var result = 0;
        foreach (var card in hand.Cards)
        {
            result += GetCardCount(card);
        }
        if (CheckIfHandIsSoft(hand, result)) {
            result += 10; //then count ace as 11
            hand.IsSoft = true;
        } 
        return result;
    }

    public static int GetCardCount(Card card)
    {
        return card.CardValue switch
        {
            CardValue.Ace => 1,
            CardValue.Two => 2,
            CardValue.Three => 3,
            CardValue.Four => 4,
            CardValue.Five => 5,
            CardValue.Six => 6,
            CardValue.Seven => 7,
            CardValue.Eight => 8,
            CardValue.Nine => 9,
            CardValue.Ten or CardValue.Jack or CardValue.Queen or CardValue.King=> 10,
            _ => throw new Exception("Böyle bir kart değeri yok.")
        };
    }

    public static bool CheckIfHandIsSoft(Hand hand, int handValue)
    {
        //If there is ace and count doesnt exceed 21 then it is a soft hand.
        return hand.Cards.Any(x => x.CardValue == CardValue.Ace) && handValue + 10 <= 21;
    }
}
