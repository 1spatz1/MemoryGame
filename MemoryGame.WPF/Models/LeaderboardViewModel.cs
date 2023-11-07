using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MemoryGame.WPF.Models;

class LeaderboardViewModel : INotifyPropertyChanged {
    private ObservableCollection<DataAccess.DataAccess.Game> _gamesCollection = new();
    private DataAccess.DataAccess _dataAccess = new();
    public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<DataAccess.DataAccess.Game> GamesCollection {
            get { return _gamesCollection; }
            set {
                _gamesCollection = value;
                OnPropertyChanged(nameof(GamesCollection));
            }
        }
        
        public LeaderboardViewModel()
        {
            var highscores = _dataAccess.GetHighscores();
            foreach (var highscore in highscores) {
                GamesCollection.Add(highscore);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }