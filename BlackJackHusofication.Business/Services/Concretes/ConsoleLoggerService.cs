using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Business.Services.Abstracts;
using BlackJackHusofication.Model.Logs;

namespace BlackJackHusofication.Business.Services.Concretes;

internal class ConsoleLoggerService : IConsoleLoggerService
{
    public Task LogMessage(SimulationLog logMessage)
    {
        return logMessage.LogType switch
        {
            SimulationLogType.CardActionLog => LogCardAction(logMessage.Message),
            SimulationLogType.CardDealLog => LogDealAction(logMessage.Message),
            SimulationLogType.GameLog => LogGameLogs(logMessage.Message),
            SimulationLogType.DealerActions => LogDealerActions(logMessage.Message),
            _ => throw new Exception("Böyle iş olmaz lan. Böyle racon olmaz")
        };
    }

    private static Task LogCardAction(string message)
    {
        return Task.Run(() =>
        {
            LogHelper.WriteLine(message, ConsoleColor.DarkCyan);
        });
    }

    private static Task LogGameLogs(string message)
    {
        return Task.Run(() =>
        {
            LogHelper.WriteLine(message, ConsoleColor.Green);
        });
    }

    private static Task LogDealAction(string message)
    {
        return Task.Run(() =>
        {
            LogHelper.WriteLine(message, ConsoleColor.Blue);
        });
    }

    private static Task LogDealerActions(string message)
    {
        return Task.Run(() =>
        {
            LogHelper.WriteLine(message, ConsoleColor.Magenta);
        });
    }
}