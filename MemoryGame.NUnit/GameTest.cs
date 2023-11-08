using MemoryGame.Logic;
using MemoryGame.Logic.Exceptions;

namespace MemoryGame.NUnit;

public class GameTest
{
    [TestCase(5, typeof(InvalidCardAmountException))]
    public void Input_InValid_CreateCards_Amount_Return_Exception(int numberOfCards, Type expectedException)
    {
        Assert.Throws(expectedException, () => Game.CreateCards(numberOfCards));
    }
    
    [TestCase(10)]
    public void Input_Valid_CreateCards_Amount_Return_Cards(int numberOfCards)
    {
        List<Card> testList = Game.CreateCards(numberOfCards);
        Dictionary<Char, int> testDict = new Dictionary<char, int>();
        foreach (var card in testList) {
            if (!testDict.ContainsKey(card.Character)) {
                testDict.TryAdd(card.Character, 1);
            }
            else {
                testDict[card.Character] += 1;
            }
        }

        foreach (var value in testDict) {
            Assert.That(value.Value, Is.EqualTo(2));
        }
    }
    
    [TestCase(10)]
    public void IsGameFinished_Returns_True(int numberOfCards)
    {
        var player = new Player("test");
        Game.StartGame(player, numberOfCards);
        foreach (var card in Game.Cards) {
            card.CardGuessed();
        }
        
        Assert.That(Game.IsGameFinished(), Is.EqualTo(true));
        Game.StopGame();
        Game.ClearGame();
    }
    
    [TestCase(10)]
    public void IsGameFinished_Returns_False(int numberOfCards)
    {
        var player = new Player("test");
        Game.StartGame(player, numberOfCards);
        foreach (var card in Game.Cards) {
            if (card.Id % 2 == 0) {
                card.CardGuessed();
            }
        }
        
        Assert.That(Game.IsGameFinished(), Is.EqualTo(false));
        Game.StopGame();
        Game.ClearGame();
    }
    
    [TestCase(10)]
    public void CompareCards_Returns_True(int numberOfCards)
    {
        var player = new Player("test");
        Game.StartGame(player, numberOfCards);
        
        Assert.That(Game.CompareCards(1, 2), Is.EqualTo(true));
        Game.StopGame();
        Game.ClearGame();
    }
    
    [TestCase(10)]
    public void CompareCards_Returns_False(int numberOfCards)
    {
        var player = new Player("test");
        Game.StartGame(player, numberOfCards);
        
        Assert.That(Game.CompareCards(3, 7), Is.EqualTo(false));
        Game.StopGame();
        Game.ClearGame();
    }
}

