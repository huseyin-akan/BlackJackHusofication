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
    public void ShouldMakeOptimalMoveForPairWhenDealerHasTwo(CardValue cardValue1, CardValue cardValue2, bool isSplitHand, CardAction expectedCardAction)
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
}
