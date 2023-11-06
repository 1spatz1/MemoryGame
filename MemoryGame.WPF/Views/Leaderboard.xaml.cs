using System.Collections.ObjectModel;
using System.Windows;

namespace MemoryGame.WPF.Views;

public partial class Leaderboard : Window
{
    public ObservableCollection<object> GamesCollection = new();
    private DataAccess.DataAccess _dataAccess = new();
    
    public Leaderboard() {
        InitializeComponent();
        AddElements(); // Voegt elemententen toe aan de ObservableCollection

        DataContext = this; // Zet DataContext op GamesCollection
    }
    
    private void AddElements() {
        var highscores = _dataAccess.GetHighscores();
        foreach (var highscore in highscores) {
            GamesCollection.Add(highscore);
        }
    }
    
    public class Game {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Date { get; set; }
        public string Turns { get; set; }
        public string AmountOfCards { get; set; }
    }
    
    private void OpenMainMenu(object sender, RoutedEventArgs e) {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}