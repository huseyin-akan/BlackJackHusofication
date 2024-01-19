using BlackJackHusofication.Managers;
using BlackJackHusofication.Models;

namespace BlackJackHusofication.UnitTests.ManagerTests;

public class OptimalMoveManagerTests
{
    [Theory]
    [InlineData(CardValue.Two, CardValue.Two, false, CardAction.Split)]
    [InlineData(CardValue.Two, CardValue.Two, true, CardAction.Hit)]
    [InlineData(CardValue.Three, CardValue.Three, false, CardAction.Split)]
    [InlineData(CardValue.Three, CardValue.Three, true, CardAction.Hit)]
    [InlineData(CardValue.Four, CardValue.Four, false, CardAction.Hit)]
    [InlineData(CardValue.Four, CardValue.Four, true, CardAction.Hit)]
    [InlineData(CardValue.Five, CardValue.Five, false, CardAction.Double)]
    [InlineData(CardValue.Five, CardValue.Five, true, CardAction.Double)]
    [InlineData(CardValue.Six, CardValue.Six, false, CardAction.Split)]
    [InlineData(CardValue.Six, CardValue.Six, true, CardAction.Hit)]
    [InlineData(CardValue.Seven, CardValue.Seven, false, CardAction.Split)]
    [InlineData(CardValue.Seven, CardValue.Seven, true, CardAction.Stand)]
    [InlineData(CardValue.Eight, CardValue.Eight, false, CardAction.Split)]
    [InlineData(CardValue.Eight, CardValue.Eight, true, CardAction.Stand)]
    [InlineData(CardValue.Nine, CardValue.Nine, false, CardAction.Split)]
    [InlineData(CardValue.Nine, CardValue.Nine, true, CardAction.Stand)]
    [InlineData(CardValue.Ten, CardValue.Jack, false, CardAction.Stand)]
    [InlineData(CardValue.Queen, CardValue.King, true, CardAction.Stand)]
    [InlineData(CardValue.Ace, CardValue.Ace, false, CardAction.Split)]
    [InlineData(CardValue.Ace, CardValue.Ace, true, CardAction.Hit)]
    public void ShouldMakeOptimalMoveForPairsWhenDealerHasTwo(CardValue cardValue1, CardValue cardValue2, bool isSplitHand, CardAction expectedCardAction)
    {
        Dealer dealer = new() { Id= 0, Name="Dealer"};
        dealer.Hand.Cards.Add(new(CardType.Hearts, CardValue.Two) );
        Hand playerHand = new();
        playerHand.Cards.Add(new(CardType.Hearts, cardValue1));
        playerHand.Cards.Add(new(CardType.Hearts, cardValue2));
        playerHand.HandValue = CardManager.GetCountOfHand(playerHand);

        var actualAction = OptimalMoveManager.MakeOptimalMove(dealer.Hand.Cards[0], playerHand, isSplitHand);

        Assert.True(actualAction == expectedCardAction);
    }

    [Theory]
    [InlineData(CardValue.Ace, CardValue.Two, CardAction.Hit)]
    [InlineData(CardValue.Ace, CardValue.Three,CardAction.Hit)]
    [InlineData(CardValue.Ace, CardValue.Four,CardAction.Hit)]
    [InlineData(CardValue.Ace, CardValue.Five, CardAction.Hit)]
    [InlineData(CardValue.Ace, CardValue.Six, CardAction.Stand)]
    [InlineData(CardValue.Ace, CardValue.Seven, CardAction.Stand)]
    [InlineData(CardValue.Ace, CardValue.Eight, CardAction.Stand)]
    [InlineData(CardValue.Ace, CardValue.Nine, CardAction.Stand)]
    public void ShouldMakeOptimalMoveForSoftHandWhenDealerHasTwo(CardValue cardValue1, CardValue cardValue2,CardAction expectedCardAction)
    {
        Dealer dealer = new() { Id = 0, Name = "Dealer" };
        dealer.Hand.Cards.Add(new(CardType.Hearts, CardValue.Two));
        Hand playerHand = new();
        playerHand.Cards.Add(new(CardType.Hearts, cardValue1));
        playerHand.Cards.Add(new(CardType.Hearts, cardValue2));
        playerHand.HandValue = CardManager.GetCountOfHand(playerHand);

        var actualAction = OptimalMoveManager.MakeOptimalMove(dealer.Hand.Cards[0], playerHand);

        Assert.True(actualAction == expectedCardAction);
    }

    [Theory]
    [InlineData(CardValue.Three, CardValue.Two, CardAction.Hit)]
    [InlineData(CardValue.Four, CardValue.Two, CardAction.Hit)]
    [InlineData(CardValue.Three, CardValue.Four, CardAction.Hit)]
    [InlineData(CardValue.Three, CardValue.Five, CardAction.Hit)]
    [InlineData(CardValue.Three, CardValue.Six, CardAction.Hit)]
    [InlineData(CardValue.Three, CardValue.Seven, CardAction.Double)]
    [InlineData(CardValue.Three, CardValue.Eight, CardAction.Double)]
    [InlineData(CardValue.Jack, CardValue.Two, CardAction.Hit)]
    [InlineData(CardValue.Ten, CardValue.Three, CardAction.Stand)]
    [InlineData(CardValue.Queen, CardValue.Four, CardAction.Stand)]
    [InlineData(CardValue.Jack, CardValue.Five, CardAction.Stand)]
    [InlineData(CardValue.Ten, CardValue.Six, CardAction.Stand)]
    [InlineData(CardValue.Queen, CardValue.Seven, CardAction.Stand)]
    [InlineData(CardValue.King, CardValue.Eight, CardAction.Stand)]
    [InlineData(CardValue.Jack, CardValue.Nine, CardAction.Stand)]
    [InlineData(CardValue.Queen, CardValue.King, CardAction.Stand)]
    [InlineData(CardValue.Jack, CardValue.Ace, CardAction.Stand)]
    public void ShouldMakeOptimalMoveForHandWhenDealerHasTwo(CardValue cardValue1, CardValue cardValue2, CardAction expectedCardAction)
    {
        Dealer dealer = new() { Id = 0, Name = "Dealer" };
        dealer.Hand.Cards.Add(new(CardType.Hearts, CardValue.Two));
        Hand playerHand = new();
        playerHand.Cards.Add(new(CardType.Hearts, cardValue1));
        playerHand.Cards.Add(new(CardType.Hearts, cardValue2));
        playerHand.HandValue = CardManager.GetCountOfHand(playerHand);

        var actualAction = OptimalMoveManager.MakeOptimalMove(dealer.Hand.Cards[0], playerHand);

        Assert.True(actualAction == expectedCardAction);
    }
}
