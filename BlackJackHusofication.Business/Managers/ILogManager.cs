namespace BlackJackHusofication.Business.Managers;

public interface ILogManager
{
    Task SendLogMessageToAllClients(string logMessage);
}