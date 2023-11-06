using MemoryGame.Logic.Exceptions;

namespace MemoryGame.Logic;

public class Player
{
    public string Name { get; private set; }
    
    public Player(string name) {
        if (string.IsNullOrEmpty(name)) {
            throw new NameIsEmptyException();
        }
        Name = name;
    }
}