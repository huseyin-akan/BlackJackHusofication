using Microsoft.AspNetCore.SignalR;

namespace BlackJackHusofication.Business.SignalR;
public class BlackJackHub : Hub
{
    static List<string> clients = []; 
    public override async  Task OnConnectedAsync() //bir client bağlanınca
    {
        // Context.ConnectionId --> Clientların bağlantı ID'sidir. Clientları bu IDler ile ayırabiliriz. Bazı mesajları sadece belirli clientlara göndermemizi sağlar.
        clients.Add(Context.ConnectionId);
        await Clients.All.SendAsync("clients", clients);
        await Clients.All.SendAsync("userJoined", Context.ConnectionId);
         
    }

    public override async Task OnDisconnectedAsync(Exception exception) //bir client kopunca
    {
        clients.Remove(Context.ConnectionId);
        await Clients.All.SendAsync("clients", clients);
        await Clients.All.SendAsync("userLeft", Context.ConnectionId);
    }
}