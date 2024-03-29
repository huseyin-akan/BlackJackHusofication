export class Card {
    cardType: CardType;
    cardValue: CardValue;
    cardImg : string;
}

export enum CardValue
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

export enum CardType
{
    Hearts = 1,
    Diamonds = 2,
    Spades = 3,
    Clubs = 4,
    ShufflerCard = -1,
    SecretCard = -2
}

export enum CardAction
{
    Hit = 1,
    Double = 2,
    Stand = 3,
    Split = 4
}