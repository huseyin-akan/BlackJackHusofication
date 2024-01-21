using Microsoft.AspNetCore.SignalR;

namespace BlackJackHusofication.Business.SignalR;
public class BlackJackHub : Hub<IBlackJackClient>
{
    static readonly List<string> clients = []; 
    public override async  Task OnConnectedAsync() //bir client bağlanınca
    {
        // Context.ConnectionId --> Clientların bağlantı ID'sidir. Clientları bu IDler ile ayırabiliriz. Bazı mesajları sadece belirli clientlara göndermemizi sağlar.
        clients.Add(Context.ConnectionId);
        await Clients.All.AllClients(clients);
        await Clients.All.UserJoined(Context.ConnectionId);
        //await Clients.All.SendAsync("clients", clients);
        //await Clients.All.SendAsync("userJoined", Context.ConnectionId);
        
        //STRONGLY TYPED HUBS :Normalde <IBlackJackClient> metodunu interface'ini vermezsek yukarıdaki gibi de kullanabiliriz. Fakat neden hataya mahal verelim ki. Tip güvenlikli çalışalım. Metot adlarımızla aynı mesajları SendAsync yapıyor kendisi arka planda. 
    }

    public override async Task OnDisconnectedAsync(Exception exception) //bir client kopunca
    {
        clients.Remove(Context.ConnectionId);
        await Clients.All.AllClients(clients);
        await Clients.All.UserLeft(Context.ConnectionId);
    }
}