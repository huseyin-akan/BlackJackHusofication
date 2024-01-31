using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Business.SignalR;
using BlackJackHusofication.Model.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlackJackHusofication.Business.BackgrounServices;

public class BjRunnerService : BackgroundService
{
    private readonly IHubContext<BlackJackGameHub, IBlackJackGameClient> _hubContext;
    private readonly ILogger<BjRunnerService> _logger;
    private readonly BjGame _game;
    private readonly BjRoomManager _bjRoomManager;

    public BjRunnerService(IServiceProvider services, int roomId)
    {
        _hubContext = services.GetRequiredService<IHubContext<BlackJackGameHub, IBlackJackGameClient>>();
        _logger = services.GetRequiredService<ILogger<BjRunnerService>>();
        _bjRoomManager = services.GetRequiredService<BjRoomManager>();
        _game = _bjRoomManager.CreateRoom($"BJ-{roomId}", roomId);
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("oyun başladı dostum");
        await Task.Delay(10_000, stoppingToken); //We wait for 10 seconds in the beginning of the game
        var cancellationToken = _game.CancellationTokenSource.Token;

        _logger.LogInformation("10 sn geçti. Hadi bakalım");

        //Game Loop
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Hadi lan kekolar bet atın");
            
            //We give to the players 30 seconds to bet. 
            var startNewRound = await CheckIfPlayersBetInTime(BjEventType.AcceptingBets, 15, cancellationToken);
            if (!startNewRound) continue;

            cancellationToken = _game.CancellationTokenSource.Token;
            _logger.LogError("Bet atan bir keko var. Gel de paranı yiyim senin enayi!");





            var message = $"Room number is {_game.RoomId}: {DateTime.Now:HH:mm}";
            await _hubContext.Clients.All.SendLog(new() { Message = message });
            _logger.LogInformation(message: message);

            //Round is over, we should reset the values.
            ResetAfterRoundEnd();
        }

        _logger.LogError("La noli hata var");
        _logger.LogDebug("hadi bakalım");
    }

    private void ResetAfterRoundEnd()
    {
        foreach (var spot in _game.Table.Spots.Where(p => p.BetAmount != 0))
        {
            spot.BetAmount = 0;
            spot.Hand = new();
            spot.SplittedHand = null;
        }
    }

    private async Task<bool> CheckIfPlayersBetInTime(BjEventType eventType, int seconds, CancellationToken cancellationToken)
    {
        try
        {
            for (int i = 0; i < seconds; i++) //each second
            {
                var delayTask = Task.Delay(1_000, cancellationToken);
                var notificationTask = _hubContext.Clients.Group(_game.Name).NotifyCountDown(new CountDownNotification(eventType, seconds - i));
                await Task.WhenAll(delayTask, notificationTask);
                _logger.LogInformation("1 sn geçti. Tüm masalarda bet yok hala. Hadi kekolar bet atın!");
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex.Message);
            _game.CancellationTokenSource = new CancellationTokenSource();
            
            return true;
        }

        _logger.LogInformation("Süre doldu. Bakalım kimse bet attı mı?");
        
        //If any player sitting in the table has betted, then start the round
        if (_game.Table.Spots.Any(x => x.BetAmount != 0) ) return true;

        return false;
    }
}