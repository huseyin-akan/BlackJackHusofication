namespace BlackJackHusofication.Models;

public class Hand
{
    public List<Card> Cards { get; set; } //TODO-HUS make this init only. And add a AddCard method which will also calculate and update HandValue
    public bool IsBlackJack { get; set; }
    public bool IsBusted { get; set; }
    public bool IsSoft { get; set; }
    public int HandValue { get; set; }  //HandValue cannot be updated from outside.
    public decimal BetAmount { get; set; }
    public Hand()
    {
        Cards = [];
    }
}

