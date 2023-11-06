using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using MemoryGame.Logic;
using MemoryGame.WPF.Models;

namespace MemoryGame.WPF.Views;

public partial class GameWindow : Window{
    public GameWindow(Player player, int numberOfCards){
        InitializeComponent();

        // Start het spel
        Game.StartGame(player, numberOfCards);

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
            image.Source = new BitmapImage(new Uri($"https://martijnschuman.nl/MemoryWPFImages/question.png"));
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
            image.Source = new BitmapImage(new Uri($"https://martijnschuman.nl/MemoryWPFImages/card_{Game.Cards[firstCardId-1]}.png"));
        }
        else if (secondCardId == 0) {
            secondCardId = clickedCardId; // Slaat het getal van de eerste kaart op
            secondButton = clickedCard; // Slaat de eerste knop op
            
            Image image = secondButton.FindName("CardImage") as Image;
            image.Source = new BitmapImage(new Uri($"https://martijnschuman.nl/MemoryWPFImages/card_{Game.Cards[secondCardId-1]}.png"));
        }
        else { // Zo niet
            // Image image = clickedCard.FindName("CardImage") as Image;
            // image.Source = new BitmapImage(new Uri($"https://martijnschuman.nl/MemoryWPFImages/card_{GameSupport.Cards[clickedCardId-1].Number}.png"));
            // Vergelijkt de kaarten
            // Doet -1 omdat de index begint bij 0, maar id van de kaarten bij 1
            if(Game.CompareCards(firstCardId-1, secondCardId-1)) {
                // Als ze gelijk zijn
                // Disable beide knoppen zodat er niet meer op gelikt kan worden
                firstButton.IsEnabled = false;
                secondButton.IsEnabled = false;

                Image image = secondButton.FindName("CardImage") as Image;
                image.Source = new BitmapImage(new Uri($"https://martijnschuman.nl/MemoryWPFImages/card_{Game.Cards[firstCardId-1]}.png"));
            }
            else {
                Image image = firstButton.FindName("CardImage") as Image;
                image.Source = new BitmapImage(new Uri($"https://martijnschuman.nl/MemoryWPFImages/question.png"));
                Image image2 = secondButton.FindName("CardImage") as Image;
                image2.Source = new BitmapImage(new Uri($"https://martijnschuman.nl/MemoryWPFImages/question.png"));
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

    private void GameFinished() {
        Game.StopGame();

        MessageBox.Show($"Het spel is afgelopen! " +
            $"\nTotaal spelduur: {Game.Time} seconden " +
            $"\nTotaal aantal pogingen: {Game.Turns}" +
            $"\nEindscore: {Game.Score}"
        );

        // Goes back to main menu
        Game.ClearGame();
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}