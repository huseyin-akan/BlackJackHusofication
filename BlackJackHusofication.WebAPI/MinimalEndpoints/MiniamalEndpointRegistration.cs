using Newtonsoft.Json;

namespace BlackJackHusofication.WebAPI.MinimalEndpoints;

public class MiniamalEndpointRegistration
{
    public static void Register(WebApplication app)
    {
        app.MapGet("/get-huso-json", () =>
            new
            {
                HusoName = "Husokanus",
                Age = 25,
                IsSexy = true,
                BirthDate = new DateTime(1992, 11, 10)
            });

        app.MapPost("/post-huso", (HusoPostInModel inModel) =>
            new
            {
                NewName = inModel.Name + "isHusofied",
                NewAge = inModel.Age + 31
            });
    }
}

internal record HusoPostInModel(string Name, int Age);
