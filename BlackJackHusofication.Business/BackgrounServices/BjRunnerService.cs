using BlackJackHusofication.Business.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlackJackHusofication.Business.BackgrounServices;

public class BjRunnerService(IServiceProvider services, int roomId) : BackgroundService
{
    private static int count = 0;
    private readonly IHubContext<BlackJackGameHub, IBlackJackGameClient> _hubContext = services.GetRequiredService<IHubContext<BlackJackGameHub, IBlackJackGameClient>>();
    private readonly ILogger<BjRunnerService> _logger = services.GetRequiredService<ILogger<BjRunnerService>>();
    private readonly int _roomId = roomId;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(3000, stoppingToken);
            var message = $"Count is : {count} - Room number is {_roomId}: {DateTime.Now:HH:mm}";
            await _hubContext.Clients.All.SendLog(new() { Message = message });
            _logger.LogInformation(message: message);
            Interlocked.Increment(ref count); //TODO-HUS bunun ne olduğunu araştıralım.
        }
    }
}
