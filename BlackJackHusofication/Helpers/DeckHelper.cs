﻿using BlackJackHusofication.Models;

namespace BlackJackHusofication.Helpers;

class DeckHelper
{
    public static List<Card> CreateFullDeck(int deckCount)
    {
        List<Card> fullDeck = [];

        for (int i = 0; i < deckCount; i++)
        {
            foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
            {
                if (cardType == CardType.ShufflerCard) continue;

                foreach (CardValue cardValue in Enum.GetValues(typeof(CardValue)))
                {
                    if (cardValue == CardValue.ShufflerCard) continue;
                    fullDeck.Add(new Card(cardType, cardValue));
                }
            }
        }
        fullDeck.Add(new Card(CardType.ShufflerCard, CardValue.ShufflerCard));

        return fullDeck;
    }

    public static List<Card> ShuffleDecks(List<Card> cards)
    {
        //Remove the shuffler card from the deck
        var shufflerCard = cards.First(x => x.CardValue == CardValue.ShufflerCard);
        var asd = cards.Where(x => x.CardValue == CardValue.ShufflerCard).ToList();
        if (shufflerCard is not null) cards.Remove(shufflerCard);

        // Shuffle the deck using Fisher-Yates algorithm
        Random random = new();
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (cards[n], cards[k]) = (cards[k], cards[n]);
        }

        //We add the shuffler card somewhere in the half
        var deckCount = cards.Count / 52;
        var somewhereInTheHalf = (cards.Count / 2) + (deckCount * 4) - random.Next(deckCount * 8);
        cards.Add(new Card(CardType.ShufflerCard, CardValue.ShufflerCard));
        (cards[^1], cards[somewhereInTheHalf]) = (cards[somewhereInTheHalf], cards[^1]);

        return cards;
    }
}