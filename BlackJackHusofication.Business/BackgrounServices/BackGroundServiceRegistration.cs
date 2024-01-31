namespace BlackJackHusofication.Business.BackgrounServices;

public static class BackGroundServiceRegistration
{
    public static async Task StartAllServices(IServiceProvider serviceProvider)
    {
        for (int i = 1; i <= 10; i++)
        {
            var roomGameService = new BjRunnerService(serviceProvider, i);
            await roomGameService.StartAsync(default); // Start the background service
        }
    }
}