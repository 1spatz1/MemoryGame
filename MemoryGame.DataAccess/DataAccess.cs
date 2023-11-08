using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace MemoryGame.DataAccess;

public class DataAccess
{
    private List<Game> _gamesList = new();

    private string GetFilePath(string input)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(currentDirectory, "../../../../MemoryGame.DataAccess/");
        return Path.Combine(filePath, input);
    }

    public int GetTotalAmountScores()
    {
        XDocument xmlDoc = XDocument.Load(GetFilePath("highscores.xml"));

        return xmlDoc.Descendants("Game").Count();
    }

    public void RemoveScore(int target)
    {
        XDocument xmlDoc = XDocument.Load(GetFilePath("highscores.xml"));
        
        xmlDoc.Descendants("Game")
            .Where(x => (int)x.Attribute("score") == target)
            .Remove();
        
        xmlDoc.Save(GetFilePath("highscores.xml"));
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

    public List<Game> GetHighscores()
    {
        try {
            XDocument xmlDoc = XDocument.Load(GetFilePath("highscores.xml"));

            var games = xmlDoc.Descendants("Game")
                .Select(gameElement => {
                    return new Game {
                        Name = gameElement.Attribute("name").Value,
                        Score = int.Parse(gameElement.Attribute("score").Value),
                        Date = gameElement.Attribute("date").Value,
                        Turns = int.Parse(gameElement.Attribute("turns").Value),
                        AmountOfCards = int.Parse(gameElement.Attribute("numberOfCards").Value)
                    };
                })
                .OrderByDescending(game => game.Score);
        
            // Adds all games to GamesCollection
            foreach (var game in games) {
                _gamesList.Add(game);
            }
        }
        catch (FileNotFoundException ex) {
            // MessageBox.Show("ERROR: \n" + ex.Message);
        }
        catch (NullReferenceException ex) { 
            // MessageBox.Show("ERROR: \n" + ex.Message); 
        }

        return _gamesList;
    }

    public List<Game> GetGamesCollection()
    {
        return _gamesList;
    }
    
    public class Game {
        public string Name { get; set; }
        public int Score { get; set; }
        public string Date { get; set; }
        public int Turns { get; set; }
        public int AmountOfCards { get; set; }
    }
}