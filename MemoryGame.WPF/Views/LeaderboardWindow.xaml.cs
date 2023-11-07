using System;
using System.Collections.ObjectModel;
using System.Windows;
using MemoryGame.WPF.Models;

namespace MemoryGame.WPF.Views;

public partial class LeaderboardWindow : Window
{
    public LeaderboardWindow() {
        InitializeComponent();

        var viewModel = new LeaderboardViewModel();
        DataContext = viewModel; // Set DataConext on LeaderbaordViewModel
    }
    
    private void OpenMainMenu(object sender, RoutedEventArgs e) {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}