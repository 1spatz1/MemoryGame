using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MemoryGame.Logic;
using MemoryGame.WPF.Views;

namespace MemoryGame.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void OpenLeaderboard(object sender, RoutedEventArgs e) {
            // Maakt een leaderboard aan en laat deze zien
            Leaderboard leaderboard = new Leaderboard();
            leaderboard.Show();
            Close();
        }
        
        private void GetGameData(object sender, RoutedEventArgs e) {
            ErrorLabel.Content = string.Empty;
            Player player = null;
            int numberOfCards = 0;

            // Als de input leeg is
            if (string.IsNullOrEmpty(NameBox.Text)) {
                // Geef een error
                ErrorLabel.Content = "Spelers naam mag niet leeg zijn!\n";
            }
            else {
                // Maak anders een speler aan
                player = new Player(NameBox.Text);
            }

            // Als de input niet leeg is
            if (!string.IsNullOrWhiteSpace(CardAmountBox.Text)) {
                // En de input is een getal, een getal niet kleiner dan 10 en een even getal
                if (int.TryParse(CardAmountBox.Text, out int result) && result >= 10 && result % 2 == 0) {
                    numberOfCards = result;
                }
                else {
                    // Geef een error
                    ErrorLabel.Content += "Voer een getal in dat groter is dan 10 en even is.";
                }
            }
            else {
                // Geef een error
                ErrorLabel.Content += "Aantal kaarten mag niet leeg zijn!";
            }

            // Ga pas verder als ze de juiste waardes zijn gezet
            if (player != null && numberOfCards != 0){
                // Maakt de gameWindow met juiste informatie
                GameWindow gameWindow = new GameWindow(player, numberOfCards);
                // Sluit huidige window  
                Close();
                // Laat de gameWindow zien
                gameWindow.Show();
            }
        }
    }
}