using MemoryGame.Logic;
using MemoryGame.Logic.Exceptions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace MemoryGame.NUnit;

public class PlayerTest
{
    [TestCase("I_HATE_MICROSOFT")]
    public void Create_Player_Return_No_Error(string name)
    {
        Player player = new Player(name);
        
        Assert.That(player, Is.Not.Null);
        Assert.That(player.Name, Is.EqualTo(name));
    }

    [TestCase("", typeof(NameIsEmptyException))]
    [TestCase(null, typeof(NameIsEmptyException))]
    public void Create_InValid_Player_Return_Exception(string name, Type expectedException)
    {
        Assert.Throws(expectedException, () => new Player(name));
    }
}