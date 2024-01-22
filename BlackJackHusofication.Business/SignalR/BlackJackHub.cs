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
        
        //STRONGLY TYPED HUBS :Normalde <IBlackJackClient> metodunu interface'ini vermezsek yukarıdaki gibi de kullanabiliriz. Fakat neden hataya mahal verelim ki. Tip güvenlikli çalışalım. Metot adlarımızla aynı mesajları SendAsync yapıyor kendisi arka planda. 

        //Clients.All yerine Clients.Caller --> isteği atan client veya Clients.Others --> istek atan hariç tüm diğer clientları da hedef olarak seçebiliriz
    }

    public override async Task OnDisconnectedAsync(Exception exception) //bir client kopunca
    {
        clients.Remove(Context.ConnectionId);
        await Clients.All.AllClients(clients);
        await Clients.All.UserLeft(Context.ConnectionId);
    }
}