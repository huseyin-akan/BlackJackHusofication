using BlackJackHusofication.Business.BackgrounServices;
using BlackJackHusofication.Business.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace BlackJackHusofication.Business;

public static class BusinessServiceRegistraiton
{
    public static void AddBusinessDependencies(this IServiceCollection services)
    {
        for (int i = 1; i <= 10; i++)
        {
            BjGameManager.CreateNewRoom($"BlackJack - {i}", i);
        }

        //services.AddHostedService<BjRunnerService>();

        for (int i = 0; i < 3; i++)
        {
            services.AddHostedService(serviceProvider => new BjRunnerService(serviceProvider, i));
        }
    }
}
