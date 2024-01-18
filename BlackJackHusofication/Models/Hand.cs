using System;

namespace BlackJackHusofication.Models;

internal class Hand
{
    public List<Card> Cards { get; set; }
    public bool IsBusted { get; set; }
    public bool IsSoft { get; set; }
    public int HandValue { get; set; }
    public Hand()
    {
        Cards = [];
    }
}

