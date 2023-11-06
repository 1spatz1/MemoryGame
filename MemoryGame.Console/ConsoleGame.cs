using MemoryGame.DataAccess.Events;
using MemoryGame.Logic;

namespace MemoryGame.Console;

public class ConsoleGame
{
    private int _firstCard;
    private int _secondCard;
    public ConsoleGame(Player player, int numberOfCards) {
        HighscoreService highscoreService = new(); // make new highscoreservice instance
        Game.GameFinished += highscoreService.OnGameFinished; // subscribe it to the game
        Game.StartGame(player, numberOfCards); // start the game

        // Zaslong as game isnt finished
        while (!Game.IsGameFinished()) {
            PrintConsoleTable(); // print all cards
            Thread.Sleep(500); // delay by duration bcs yeah
            AskConsoleInput(); // ask for input
        }

        Game.StopGame(); // stop the game
        PrintConsoleFinished(); // print the score
    }

    // print the score
    public void PrintConsoleFinished() {
        System.Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-==-=-=-=-=-=-=-=-=-");

        System.Console.WriteLine("The game finished");
        System.Console.WriteLine($"\tTotal duration: {Math.Round(Game.Time)} seconden");
        System.Console.WriteLine($"\tTotal tries: {Game.Turns}");
        System.Console.WriteLine($"\tScore: {Math.Round(Game.Score)}");
    }

    // print the board
    private void PrintConsoleTable() {
        System.Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-==-=-=-=-=-=-=-=-=-");

        // for every card
        for (int i = 0; i < Game.Cards.Count; i++) {
            // check if card is guessed or if it's one of the turn's cards
            string status = Game.Cards[i].IsGuessed || i == _firstCard-1 || i == _secondCard-1 ? Game.Cards[i].Character.ToString() : "XX";
            string Number = (i + 1).ToString("00");
            // string Number = Game.Cards[i].Id.ToString("00");

            System.Console.Write($"{status} ({Number})| ");

            // if it's the 5th element of row make new row
            if ((i + 1) % 5 == 0) {
                System.Console.WriteLine();
            }
        }

        System.Console.WriteLine();
        System.Console.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-==-=-=-=-=-=-=-=-=-");
    }

    // validate console input
    private int GetValidCardInput(string nummer) {
        int cardIndex = -1;

        // aslong as input is invalid, keep looping
        while (cardIndex < 0 || cardIndex > Game.Cards.Count) {
            System.Console.WriteLine($"Give the number of the {nummer} card");
            string input = System.Console.ReadLine();

            if (int.TryParse(input, out int tempIndex)) {
                cardIndex = tempIndex;
            }
        }

        return cardIndex;
    }

    // Ask for console input
    private void AskConsoleInput() {
        if (_firstCard == 0) {
            _firstCard = GetValidCardInput("first");
        }
        else if (_secondCard == 0) {
            _secondCard = GetValidCardInput("second");
        }
        else {
            Game.CompareCards(_firstCard-1, _secondCard-1);
            _firstCard = 0;
            _secondCard = 0;
        }
        
    }
}