namespace BlackJackHusofication.Business.BackgroundServices;

public static class BackGroundServiceRegistration
{
    public static async Task StartAllServices(IServiceProvider serviceProvider)
    {
        for (int i = 1; i <= 1; i++) //TODO-HUS oda sayısını 10'a çıkarıcaz.
        {
            var roomGameService = new BjRunnerService(serviceProvider, i);
            await roomGameService.StartAsync(default); // Start the background service
        }
    }
}