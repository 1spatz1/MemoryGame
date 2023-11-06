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
    // Getter en setter voor de viewCardsList
    private ObservableCollection<Card> viewCardsList = new ();
    public ObservableCollection<Card> ViewCardsList {
        get { return viewCardsList; }
        set {
            viewCardsList = value;
            OnPropertyChanged(nameof(ViewCardsList)); // Zorgt ervoor dat de WPF UI wordt bijgewerkt
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // Constructor die de ObservableCollection vult met de kaarten
    public GameViewModel(int numberOfCards, List<Card> Cards) {
        // Vul de ObservableCollection met kaarten uit de gegeven lijst
        for (int i = 0; i < numberOfCards; i++) {
            ViewCardsList.Add(Cards[i]);
        }
    }

    // Wordt aangeroepen om wijzigingen in eigenschappen door te geven
    protected virtual void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}