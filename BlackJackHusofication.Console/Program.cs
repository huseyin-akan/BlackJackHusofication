using BlackJackHusofication.Business.Managers;

GameManager gameManager = new(null, null);
gameManager.StartNewGame();

bool isExitGame = false;
while (!isExitGame)
{
    var roundToPlay = GameManager.AskForRounds();
    if (roundToPlay == 0) isExitGame = true;
    gameManager.PlayRounds(roundToPlay);
}