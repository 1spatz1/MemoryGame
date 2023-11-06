namespace MemoryGame.Logic.Exceptions;

public class InvalidCardAmountException : Exception
{
    public InvalidCardAmountException() :
        base("Amount of cards cannot be lower than 10 and has to be an even value ") { }
    public InvalidCardAmountException(string message) :
        base(message) { }
    public InvalidCardAmountException(string message, Exception innerException) :
        base(message, innerException) { }
}