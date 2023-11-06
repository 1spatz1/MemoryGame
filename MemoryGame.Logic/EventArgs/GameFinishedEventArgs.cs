namespace MemoryGame.Logic.EventArgs;

public class GameFinishedEventArgs
{
    public string PlayerName;
    public DateTime Date;
    public double Score;
    public int Turns;
    public int CardAmount;
}