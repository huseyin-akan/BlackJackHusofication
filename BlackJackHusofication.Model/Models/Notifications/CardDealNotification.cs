namespace BlackJackHusofication.Model.Models.Notifications;

public class CardDealNotification
{
    public int SpotNo { get; set; } //If spotNo is 0 then it is for dealer.
    public required Card NewCard { get; set; }
    public required int HandValue { get; set; }
}
