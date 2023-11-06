namespace MemoryGame.Logic.Exceptions;

public class NameIsEmptyException : Exception
{
    public NameIsEmptyException() :
        base("Name cannot be empty!") { }
    public NameIsEmptyException(string message) :
        base(message) { }
    public NameIsEmptyException(string message, Exception innerException) :
        base(message, innerException) { }
}