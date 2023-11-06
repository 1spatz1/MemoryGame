using MemoryGame.Logic;

namespace MemoryGame.Console;
public class Program {
    public static void Main(string[] args) {
        System.Console.WriteLine("-=-=-=-=-=-=-=-= Memory Game! =-=-=-=-=-=-=-=-=-");

        new ConsoleGame(getConsolePlayerData(), getConsoleGameData());
    }

    public static Player getConsolePlayerData() {
        System.Console.WriteLine("Give the name of the player");
        string playerName = System.Console.ReadLine();
        while (string.IsNullOrWhiteSpace(playerName)) {
            System.Console.WriteLine("Give a valid name");
            playerName = System.Console.ReadLine();
        }

        return new Player(playerName);
    }

    public static int getConsoleGameData() {
        System.Console.WriteLine("Give the amount of cards (can't be smaller than 10):");

        while (true) {
            string consoleInput = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(consoleInput)) {
                System.Console.WriteLine("Invalid Input. Give a number greater than 10");
                continue; // Go to the next iteration
            }

            if (int.TryParse(consoleInput, out int numberOfCards) && numberOfCards >= 10 && numberOfCards % 2 == 0) {
                return numberOfCards;
            }
            System.Console.WriteLine("Invalid Input. Give a number greater than 10.");
        }
    }
}