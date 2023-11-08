using MemoryGame.Logic.EventArgs;

namespace MemoryGame.DataAccess.Events;

public class HighscoreService
{
    private DataAccess _dataAccess = new();
    public void OnGameFinished(GameFinishedEventArgs e)
    {
        _dataAccess.GetHighscores();
        while (_dataAccess.GetTotalAmountScores() >= 10) {
            var lowestScore = _dataAccess.GetGamesCollection().Min(game => game.Score);
            if (e.Score > lowestScore) {
                _dataAccess.RemoveScore(lowestScore);
            }
        }
        _dataAccess.AddScore(e.PlayerName, e.Score, e.Date, e.Turns, e.CardAmount);
    }
}