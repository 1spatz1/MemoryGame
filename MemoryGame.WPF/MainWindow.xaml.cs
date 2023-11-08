using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;
using MemoryGame.WPF.Functions;

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
            // Make new leaderboard and show
            LeaderboardWindow leaderboard = new LeaderboardWindow();
            leaderboard.Show();
            Close();
        }
        
        private void UploadImages(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pictures|*.png;*.jpg;*.webp;*.jpeg";
            openFileDialog.Multiselect = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            bool? response = openFileDialog.ShowDialog();

            if(response == true) {
                foreach (String file in openFileDialog.FileNames) {
                    var fullPath = file;
                    string[] partsFileName = fullPath.Split('\\');
                    var image= partsFileName[(partsFileName.Length - 1)];
                    SaveImages(image, fullPath);
                }
            }
        }

        public void SaveImages(string image, string fullPath)
        {
            string destinationPath = GetDestinationPath(image, @"..\..\..\Assets\CardImages");

            File.Copy(fullPath, destinationPath, true);
        }
        
        private static String GetDestinationPath(string filename, string foldername)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }
        
        private void GetGameData(object sender, RoutedEventArgs e) {
            ErrorLabel.Content = string.Empty;
            Player player = null;
            int numberOfCards = 0;

            // If input is empty
            if (string.IsNullOrEmpty(NameBox.Text)) {
                // Give error
                ErrorLabel.Content = "Player name can't be empty!\n";
            }
            else {
                // Make new player
                player = new Player(NameBox.Text);
            }

            // If input is not empty
            if (!string.IsNullOrWhiteSpace(CardAmountBox.Text)) {
                // If input is a number, but not smaller than 10 and is even
                if (int.TryParse(CardAmountBox.Text, out int result) && result >= 10 && result % 2 == 0) {
                    int imagesCount;
                    if (result / 2 <= (imagesCount = GetFilesFunction.GetFiles("../../../Assets/CardImages/", @"\.jpg|\.png|\.webp|\.jpeg/gmisx").Count())) {
                        numberOfCards = result;
                    }
                    else {
                        // Give error
                        ErrorLabel.Content += $"Please upload {result / 2 - imagesCount} images, if you want to play with {result}";
                    }
                }
                else {
                    // Give error
                    ErrorLabel.Content += "Enter a number that's greater than 10 and even.";
                }
            }
            else {
                // Give error
                ErrorLabel.Content += "Amount of cards can't be empty!";
            }

            // If player isnt null and numberofcards isnt 0
            if (player != null && numberOfCards != 0){
                // make new gameWindow with info
                GameWindow gameWindow = new GameWindow(player, numberOfCards);
                // Close current window
                Close();
                // Show gamewindow
                gameWindow.Show();
            }
        }
    }
}