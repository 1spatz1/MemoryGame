using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using MemoryGame.DataAccess.Events;
using MemoryGame.Logic;
using MemoryGame.WPF.Models;

namespace MemoryGame.WPF.Views;

public partial class GameWindow : Window{
    public GameWindow(Player player, int numberOfCards){
        InitializeComponent();
        
        HighscoreService highscoreService = new(); // make new highscoreservice instance
        Game.GameFinished += highscoreService.OnGameFinished; // subscribe it to the game

        // Start het spel
        Game.StartGame(player, numberOfCards);
        
        //add images to cards
        AddImages(Game.Cards);

        // Zet de dataContext voor de UI; welke data hij moet gebruiken
        DataContext = new GameViewModel(numberOfCards, Game.Cards); 
    }

    // Methode die het klikken op een kaart verder afhandeld
    private int firstCardId = 0; // Eerste kaart wordt opgeslagen zodat deze kan worden vergeleken met de tweede
    private Button firstButton = null; // Eerste button wordt opgeslagen zodat deze later kan worden bewerkt kan worden
    private int secondCardId = 0;
    private Button secondButton = null;
    private void CardChosen(object sender, RoutedEventArgs e) {
        Button clickedCard = (Button)sender; // Zet de sender om in een button
        int clickedCardId = int.Parse(clickedCard.Tag.ToString()); // Pakt de waarde uit de button om zo juiste kaart op te halen

        // Als er twee keer op dezelfde knop wordt gedrukt
        if(firstButton == clickedCard) {
            // Reset alles
            Image image = clickedCard.FindName("CardImage") as Image;
            image.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
            firstCardId = 0;
            firstButton = null;
            clickedCardId = 0;
            clickedCard = null;
            return;
        }
        else if (firstCardId == 0) { // Als het om de eerste van de twee kaarten gaat
            firstCardId = clickedCardId; // Slaat het getal van de eerste kaart op
            firstButton = clickedCard; // Slaat de eerste knop op

            Image image = firstButton.FindName("CardImage") as Image;
            image.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[firstCardId-1].Image}", UriKind.Relative));
        }
        else if (secondCardId == 0) {
            secondCardId = clickedCardId; // Slaat het getal van de eerste kaart op
            secondButton = clickedCard; // Slaat de eerste knop op
            
            Image image = secondButton.FindName("CardImage") as Image;
            image.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[secondCardId-1].Image}", UriKind.Relative));
        }
        else { // Zo niet
            // Vergelijkt de kaarten
            // Doet -1 omdat de index begint bij 0, maar id van de kaarten bij 1
            if(Game.CompareCards(firstCardId-1, secondCardId-1)) {
                // Als ze gelijk zijn
                // Disable beide knoppen zodat er niet meer op gelikt kan worden
                firstButton.IsEnabled = false;
                secondButton.IsEnabled = false;

                Image image = secondButton.FindName("CardImage") as Image;
                image.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[firstCardId-1].Image}", UriKind.Relative));
            }
            else {
                Image image = firstButton.FindName("CardImage") as Image;
                image.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
                Image image2 = secondButton.FindName("CardImage") as Image;
                image2.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
            }

            // Reset de kaarten 
            firstCardId = 0;
            firstButton = null;
            clickedCardId = 0;
            clickedCard = null;
            secondCardId = 0;
            secondButton = null;

            // Als alle kaarten gegokt zijn
            if (Game.IsGameFinished()) {
                GameFinished();
            }
        }
    }

    private void AddImages(List<Card> Cards)
    {
        Dictionary<char, string> addedImages = new Dictionary<char, string>();
        List<string> images = GetFiles("../../../Assets/CardImages/", @"\.jpg|\.png|\.webp|\.jpeg/gmisx").ToList();
        images.Shuffle();
        foreach (var card in Game.Cards) {
            if (!addedImages.ContainsKey(card.Character)) {
                card.SetImage(images.First());
                addedImages.TryAdd(card.Character, images.First());
                images.RemoveRange(0, 1);
            }
            else {
                addedImages.TryGetValue(card.Character, out var cardString);
                card.SetImage(cardString);
            }
        }
    }
    
    public static IEnumerable<string> GetFiles(string path, string searchPatternExpression, SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        Regex reSearchPattern = new Regex(searchPatternExpression);
        return Directory.EnumerateFiles(path, "*", searchOption).Where(file => reSearchPattern.IsMatch(Path.GetFileName(file)));
    }

    public static List<string> GetFileNames(string path, string searchPatternExpression)
    {
        // Take a snapshot of the file system.  
        DirectoryInfo dir = new DirectoryInfo(path);  
  
        // This method assumes that the application has discovery permissions  
        // for all folders under the specified path.  
        IEnumerable<FileInfo> fileList = dir.GetFiles("*.*", SearchOption.TopDirectoryOnly);
  
        //Create the query  
        Regex reSearchPattern = new Regex(searchPatternExpression);

        IEnumerable<FileInfo> fileQuery =
            from file in fileList
            where reSearchPattern.IsMatch(file.Name)
            select file;
        
        return fileQuery.Select(file => file.Name).ToList();
    }
    
    private void GameFinished() {
        Game.StopGame();

        MessageBox.Show($"Het spel is afgelopen! " +
            $"\nTotaal duration: {Game.Time} seconden " +
            $"\nTotaal attempts: {Game.Turns}" +
            $"\nScore: {Game.Score}"
        );

        // Goes back to main menu
        Game.ClearGame();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}