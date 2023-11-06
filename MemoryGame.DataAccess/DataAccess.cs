using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace MemoryGame.DataAccess;

public class DataAccess
{
    private Collection<Game> _gamesCollection = new();

    private string GetFilePath(string input)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var idk = Path.Combine(currentDirectory, "../../../../MemoryGame.DataAccess/");
        return Path.Combine(idk, input);
    }

    public int GetTotalAmountScores()
    {
        XDocument xmlDoc = XDocument.Load(GetFilePath("highscores.xml"));

        return xmlDoc.Descendants("Leaderboard").Count();
    }

    public void RemoveScore(string target)
    {
        XDocument xmlDoc = XDocument.Load(GetFilePath("highscores.xml"));
        
        xmlDoc.Descendants("Leaderboard")
            .Elements("Game")
            .Where(x => x.Value == target)
            .Remove();
    }
    
    public void AddScore(string playerName, double score, DateTime date, int turns, int cardAmount)
    {
        XDocument xmlDoc = XDocument.Load(GetFilePath("highscores.xml"));
        
        xmlDoc.Element("Leaderboard").Add(new XElement("Game",
            new XAttribute("name", playerName),
            new XAttribute("score", score),
            new XAttribute("date", date.ToString("dd-MM-yyyy-HH:mm")),
            new XAttribute("turns", turns),
            new XAttribute("numberOfCards", cardAmount)
        ));
        
        xmlDoc.Save(GetFilePath("highscores.xml"));
    }

    public Collection<Game> GetHighscores()
    {
        try {
            XDocument xmlDoc = XDocument.Load(GetFilePath("highscores.xml"));
            List<string> gamesList = null;

            var games = xmlDoc.Descendants("Game")
                .Select(gameElement => {
                    return new Game {
                        Name = gameElement.Attribute("name").Value,
                        Score = int.Parse(gameElement.Attribute("score").Value),
                        Date = gameElement.Attribute("date").Value,
                        Turns = int.Parse(gameElement.Attribute("turns").Value),
                        NumberOfCards = int.Parse(gameElement.Attribute("numberOfCards").Value)
                    };
                })
                .OrderByDescending(game => game.Score);
        
            // Adds all games to GamesCollection
            foreach (var game in games) {
                _gamesCollection.Add(game);
            }
        }
        catch (FileNotFoundException ex) {
            // MessageBox.Show("ERROR: \n" + ex.Message);
        }
        catch (NullReferenceException ex) { 
            // MessageBox.Show("ERROR: \n" + ex.Message); 
        }

        return _gamesCollection;
    }

    public Collection<Game> GetGamesCollection()
    {
        return _gamesCollection;
    }
    
    public class Game {
        public string Name { get; set; }
        public double Score { get; set; }
        public string Date { get; set; }
        public int Turns { get; set; }
        public int NumberOfCards { get; set; }
    }
}