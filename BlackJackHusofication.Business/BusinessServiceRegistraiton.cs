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
    }
}
