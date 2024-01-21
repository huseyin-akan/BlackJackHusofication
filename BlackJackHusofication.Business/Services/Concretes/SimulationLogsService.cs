using BlackJackHusofication.Business.Services.Abstracts;
using BlackJackHusofication.Business.SignalR;
using BlackJackHusofication.Model.Logs;
using BlackJackHusofication.Model.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlackJackHusofication.Business.Services.Concretes;

public class SimulationLogsService(IHubContext<BlackJackHub> hubContext) : ISimulationLogsService
{
    private readonly IHubContext<BlackJackHub> _hubContext = hubContext;
    public async Task LogMessage(SimulationLog logMessage)
    {
        await _hubContext.Clients.All.SendAsync("SendLog", logMessage); 
    }

    public async Task UpdateSimulation(BjSimulation simulation)
    {
        await _hubContext.Clients.All.SendAsync("UpdateSimulation", simulation);
    }
}