using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MemoryGame.Logic;

namespace MemoryGame.WPF.Models;

/* EVENT VOLGORDE
 *
 * Voegt kaart toe aan de ObservableCollection op regel 36
 * Door het toeveogen wordt OnPropertyChanged op regel 26 uitgevoerd
 * Deze activeert het event PropertyChanged op regel 30
 * Deze event werkt de UI automatisch bij
 *
 */


class GameViewModel : INotifyPropertyChanged {
    // Getter and Setter viewCardsList
    private ObservableCollection<Card> viewCardsList = new ();
    public ObservableCollection<Card> ViewCardsList {
        get { return viewCardsList; }
        set {
            viewCardsList = value;
            OnPropertyChanged(nameof(ViewCardsList)); // Makes it WPF UI gets changed
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // Constructor that fills ObservableCollection with cards
    public GameViewModel(int numberOfCards, List<Card> Cards) {
        // fill the ObservableCollection with cards from list
        for (int i = 0; i < numberOfCards; i++) {
            ViewCardsList.Add(Cards[i]);
        }
    }

    // gets called to change properties
    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}