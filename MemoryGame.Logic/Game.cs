using System.Diagnostics;
using MemoryGame.Logic.EventArgs;
using MemoryGame.Logic.Exceptions;

namespace MemoryGame.Logic;

public static class Game
{
    private static Stopwatch _stopWatch = new ();
    public static double Time { get; private set; }
    public static Player CurrentPlayer { get; private set; }
    public static List<Card> Cards { get; private set; }
    public static int Turns { get; private set; }
    public static double Score { get; private set; }
        
    public delegate void GameFinishedEventHandler(GameFinishedEventArgs e);
    public static event GameFinishedEventHandler GameFinished;
        
    public static void OnGameFinished()
    {
        if (GameFinished != null) GameFinished(new GameFinishedEventArgs { PlayerName = CurrentPlayer.Name, Score = Score, Date = DateTime.Now, Turns = Turns, CardAmount = Cards.Count });
    }
    
    // Creates cards
    public static List<Card> CreateCards(int numberOfCards) 
    {
        List<Card> Cards = new List<Card>();
        HashSet<uint> set = new HashSet<uint>();
        Random random = new();
        int cardId = 0;

        if (numberOfCards % 2 != 0) throw new InvalidCardAmountException();
        // creates number of unique characters
        numberOfCards /= 2;
        while (set.Count != numberOfCards) {
            var randNumber = random.Next(33, 127);
            set.Add((uint)randNumber);
        }
        
        //foreach character create 2 cards
        foreach (var item in set) {
            cardId++;
            Cards.Add(FactoryOfCard(cardId, item));
            cardId++;
            Cards.Add(FactoryOfCard(cardId, item));
        }

        // shuffles cards
        Cards.Shuffle();

        return Cards;
    }
    
    // Start the game
    public static void StartGame(Player player, int amountOfCards)
    {
        _stopWatch.Start();
        CurrentPlayer = player;
        Cards = CreateCards(amountOfCards);
    }
    
    // stops the game
    public static void StopGame() 
    {
        _stopWatch.Stop(); // stops timer
        Time = Math.Round(_stopWatch.Elapsed.TotalSeconds); // saves time
        Score = CalculateGameScore(); // calculates score
        OnGameFinished();
        _stopWatch.Reset();
        Turns = 0;
        Cards.Clear();
    }
    
    public static void ClearGame()
    {
        _stopWatch.Reset();
        Turns = 0;
        Cards.Clear();
    }
    
    // Check if game is finished
    public static bool IsGameFinished() {
        // Loops all cards
        foreach(Card card in Cards) {
            // If a card isn't guessed
            if (!card.IsGuessed) {
                // Returns false en stops the foreach
                return false; 
            }
        }

        // ALl cards are guessed
        return true;
    }
    
    public static bool CompareCards(int firstCardId, int secondCardId) {
        Turns++;

        if (Cards[firstCardId].Character != Cards[secondCardId].Character) return false;
        Cards[firstCardId].CardGuessed();
        Cards[secondCardId].CardGuessed();
        
        return true;
    }
    
    public static int CalculateGameScore()
    {
        return (int)(Math.Pow(Cards.Count, 2) / (Time * Turns) * 1000);
    }
    
    private static Card FactoryOfCard(int id, uint characterInt)
    {
        return new Card(id, characterInt);
    }
}