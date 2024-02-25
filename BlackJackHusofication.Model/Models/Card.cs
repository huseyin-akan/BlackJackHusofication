using System.Diagnostics.CodeAnalysis;

namespace BlackJackHusofication.Model.Models;

public record Card
{
    public required CardType CardType { get; init; }
    public required CardValue CardValue { get; init; }
    public string? CardImg { get; init; }

    [SetsRequiredMembers]
    public Card(CardType cardType, CardValue cardValue, string? cardImg)
    {
        CardType = cardType;
        CardValue = cardValue;
        CardImg = cardImg;
    }

    public Card() { }

    public static Card CreateSecretCard()
    {
        return new Card()
        {
            CardType = CardType.SecretCard,
            CardValue = CardValue.SecretCard,
            CardImg = "card-back.jpg",
        };
    }

    public static Card CreateShufflerCard()
    {
        return new Card()
        {
            CardType = CardType.ShufflerCard,
            CardValue = CardValue.ShufflerCard,
            CardImg = "shuffler-card.svg"
        };
    }
};

public enum CardValue
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    ShufflerCard = -1,
    SecretCard = -2
}

public enum CardType
{
    Hearts = 1,
    Diamonds = 2,
    Spades = 3,
    Clubs = 4,
    ShufflerCard = -1,
    SecretCard = -2
}

public enum CardAction
{
    Hit = 1,
    Double = 2,
    Stand = 3,
    Split = 4
}