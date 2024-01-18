using BlackJackHusofication.Managers;

GameManager game = new();
var roundsToPlay = GameManager.AskForRounds();

game.PlayRounds(roundsToPlay);