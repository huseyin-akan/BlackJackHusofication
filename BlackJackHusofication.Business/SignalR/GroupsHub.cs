using Microsoft.AspNetCore.SignalR;

namespace BlackJackHusofication.Business.SignalR;

public class GroupsHub : Hub
{
    public Task Join(string groupName) => Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    public Task Leave(string groupName) => Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    public Task Message(string groupName) => Clients.Groups(groupName).SendAsync("group_message", new {husoNumber =  256});
}
