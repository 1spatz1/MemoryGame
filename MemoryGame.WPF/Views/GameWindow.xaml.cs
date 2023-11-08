using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

    // Method that processes card clicks
        private int firstCardId = 0; // first card gets saved
        private Button firstButton = null; // first button gets saved
        private async void CardChosen(object sender, RoutedEventArgs e) {
            Button clickedCard = (Button)sender;
            int clickedCardId = int.Parse(clickedCard.Tag.ToString());
            Image clickedCardImage = clickedCard.FindName("CardImage") as Image;

            // if same card gets pressed twice
            if (firstButton == clickedCard) {
                // Reset everything
                clickedCardImage.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
                firstCardId = 0;
                firstButton = null;
                return;
            }
            // if its first card
            else if (firstCardId == 0) {
                // save card information
                firstCardId = clickedCardId;
                firstButton = clickedCard;

                // show card
                Image image = firstButton.FindName("CardImage") as Image;
                image.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[firstCardId -1].Image}", UriKind.Relative));
            }
            else { // If its the second
                // show card
                clickedCardImage.Source = new BitmapImage(new Uri($"../Assets/CardImages/{Game.Cards[clickedCardId -1].Image}", UriKind.Relative));

                // Compare cards
                if (Game.CompareCards(firstCardId - 1, clickedCardId - 1)) {
                    // if they the same disable buttons
                    firstButton.IsEnabled = false;
                    clickedCard.IsEnabled = false;
                }
                else {
                    // Wait before cards get reset
                    await ResetCardsAfterDelay(firstButton, clickedCard);
                }

                // If all cards are guessed
                if (Game.IsGameFinished()) {
                    GameFinished();
                }

                // Reset the temperary values
                firstCardId = 0;
                firstButton = null;
            }
        }
    
    // Reset images after 500ms
    private async Task ResetCardsAfterDelay(Button firstCard, Button secondCard) {
        await Task.Delay(500);

        Image image = firstCard.FindName("CardImage") as Image;
        image.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
        Image image2 = secondCard.FindName("CardImage") as Image;
        image2.Source = new BitmapImage(new Uri("../Assets/shinji.jpg", UriKind.Relative));
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