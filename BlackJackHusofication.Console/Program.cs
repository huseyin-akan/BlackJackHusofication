using BlackJackHusofication.Business.Managers;

BjSimulationManager gameManager = new(null, null);
gameManager.StartNewGame();

bool isExitGame = false;
while (!isExitGame)
{
    var roundToPlay = BjSimulationManager.AskForRounds();
    if (roundToPlay == 0) isExitGame = true;
    gameManager.PlayRounds(roundToPlay);
}