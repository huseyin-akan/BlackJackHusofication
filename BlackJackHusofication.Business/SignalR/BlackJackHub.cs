using Microsoft.AspNetCore.SignalR;

namespace BlackJackHusofication.Business.SignalR;
public class BlackJackHub : Hub
{
    public async Task SendLog2(string logMessage)
    {
        // Broadcast the log message to all connected clients
        await Clients.All.SendAsync("SendLog", logMessage);
    }

    public async Task AcceptBets2(int amount)
    {
        // Process the bet logic here, you may want to validate and update the game state
        // For now, let's broadcast the bet amount to all connected clients
        await Clients.All.SendAsync("AcceptBets", amount);
    }

    public async Task GetUserAction2(string action, object data)
    {
        // Process user actions here (e.g., Stand, Hit, Double)
        // Update the game state accordingly
        // Broadcast the updated game state to all connected clients
        await Clients.All.SendAsync("GetUserAction", action, data);
    }
}