using BlackJackHusofication.Business.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace BlackJackHusofication.Business.Managers;

public class LoggerManager : ILogManager
{
    private readonly IHubContext<BlackJackHub> _hubContext;

    public LoggerManager(IHubContext<BlackJackHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendLogMessageToAllClients(string logMessage)
    {
        await _hubContext.Clients.All.SendAsync("SendLog", logMessage);
    }
}
