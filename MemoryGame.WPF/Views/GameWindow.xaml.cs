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
using MemoryGame.WPF;
using MemoryGame.WPF.Functions;

namespace MemoryGame.WPF.Views;

public partial class GameWindow : Window{
    public GameWindow(Player player, int numberOfCards){
        InitializeComponent();
        
        HighscoreService highscoreService = new(); // make new highscoreservice instance
        Game.GameFinished += highscoreService.OnGameFinished; // subscribe it to the game

        // Start the game
        Game.StartGame(player, numberOfCards);
        
        //add images to cards
        AddImages(Game.Cards);

        // set dataContext for the UI;
        DataContext = new GameViewModel(numberOfCards, Game.Cards); 
    }

    // Method for processing card clicks
    private int firstCardId = 0; // First card get saved
    private Button firstButton = null; // First button gets saved
    private int secondCardId = 0; // Second card get saved
    private Button secondButton = null; // Second button gets saved
    private void CardChosen(object sender, RoutedEventArgs e) {
        Button clickedCard = (Button)sender; // Set the sender into a button
        int clickedCardId = int.Parse(clickedCard.Tag.ToString()); // Grab the value from button to get the right card

        // If same button gets pressed twice
        if(firstButton == clickedCard) {
            // Reset everything
            Image image = clickedCard.FindName("CardImage") as Image;
            image.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
            firstCardId = 0;
            firstButton = null;
            clickedCardId = 0;
            clickedCard = null;
            return;
        }
        else if (firstCardId == 0) { // If first card is not chosen yet
            firstCardId = clickedCardId; // Save the Id of the first card
            firstButton = clickedCard; // Save the button

            // set button images to card image
            Image image = firstButton.FindName("CardImage") as Image;
            image.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[firstCardId-1].Image}", UriKind.Relative));
        }
        else if (secondCardId == 0) { // If second card is not chosen yet
            secondCardId = clickedCardId; // Save the Id of the second card
            secondButton = clickedCard; // Save the button
            
            // set button images to card image
            Image image = secondButton.FindName("CardImage") as Image;
            image.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[secondCardId-1].Image}", UriKind.Relative));
        }
        else {
            // Compare the cards
            // Do -1 because indes starts at 0, but id starts at 1
            if(Game.CompareCards(firstCardId-1, secondCardId-1)) {
                // if they are even
                // disable buttons, so they not selectable anymore
                firstButton.IsEnabled = false;
                secondButton.IsEnabled = false;

                // Image image = secondButton.FindName("CardImage") as Image;
                // image.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[firstCardId-1].Image}", UriKind.Relative));
            }
            else {
                // set button images back to the closed image
                Image image = firstButton.FindName("CardImage") as Image;
                image.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
                Image image2 = secondButton.FindName("CardImage") as Image;
                image2.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
            }

            // Reset the cards 
            firstCardId = 0;
            firstButton = null;
            clickedCardId = 0;
            clickedCard = null;
            secondCardId = 0;
            secondButton = null;

            // If all cards are open
            if (Game.IsGameFinished()) {
                GameFinished();
            }
        }
    }

    private void AddImages(List<Card> Cards)
    {
        Dictionary<char, string> addedImages = new Dictionary<char, string>();
        List<string> images = GetFilesFunction.GetFiles("../../../Assets/CardImages/", @"\.jpg|\.png|\.webp|\.jpeg/gmisx").ToList();
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
    
    private void GameFinished() {
        Game.StopGame();

        MessageBox.Show($"The game is finised!! " +
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