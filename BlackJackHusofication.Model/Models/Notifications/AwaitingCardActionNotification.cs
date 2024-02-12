namespace BlackJackHusofication.Model.Models.Notifications;

public class AwaitingCardActionNotification
{
    public required int Seconds { get; set; }
    public required int SpotNo { get; set; }
    public bool IsForSplitHand { get; set; }
}