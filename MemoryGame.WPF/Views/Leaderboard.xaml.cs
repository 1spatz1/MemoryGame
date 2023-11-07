using System;
using System.Collections.ObjectModel;
using System.Windows;
using MemoryGame.WPF.Models;

namespace MemoryGame.WPF.Views;

public partial class Leaderboard : Window
{
    public ObservableCollection<DataAccess.DataAccess.Game> GamesCollection = new();
    private DataAccess.DataAccess _dataAccess = new();
    
    public Leaderboard() {
        InitializeComponent();
        AddElements(); // Voegt elemententen toe aan de ObservableCollection

        DataContext = GamesCollection; // Zet DataContext op GamesCollection
    }
    
    private void AddElements() {
        var highscores = _dataAccess.GetHighscores();
        foreach (var highscore in highscores) {
            Console.WriteLine(highscore.ToString());
            // Game game = (Game)Convert.ChangeType(highscore, typeof(Game));
            GamesCollection.Add(highscore);
        }
    }
    
    private void OpenMainMenu(object sender, RoutedEventArgs e) {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}