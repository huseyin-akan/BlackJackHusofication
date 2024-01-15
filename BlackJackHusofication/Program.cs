using BlackJackHusofication.Managers;

GameManager game = new();
game.StartNewGame();

var roundsToPlay = GameManager.AskForRounds();

game.PlayRounds(roundsToPlay);