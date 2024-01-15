namespace BlackJackHusofication.Models;

internal class Hand
{
    public List<Card> Cards { get; set; }
    public bool IsBusted { get; set; }
    public Hand()
    {
        Cards = [];
    }
}
