namespace MemoryGame.Logic;

public class Card
{
    public int Id { get; private set; }
    public char Character { get; private set; }
    public bool IsGuessed { get; private set; }

    public Card(int id, uint characterInt) {
        Id = id;
        Character = Convert.ToChar(characterInt);
    }

    public void CardGuessed() {
        IsGuessed = true;
    }
}