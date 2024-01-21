using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Business.Services.Concretes;

BjSimulationManager gameManager = new(new ConsoleLoggerService() );
await gameManager.StartNewGame();

bool isExitGame = false;
while (!isExitGame)
{
    var roundToPlay = BjSimulationManager.AskForRounds();
    if (roundToPlay == 0) isExitGame = true;
    await gameManager.PlayRounds(roundToPlay);
}