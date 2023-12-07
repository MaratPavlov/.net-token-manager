namespace ExtTokenKeeper.Exceptions;

public class CantGetTokenException : Exception
{
    public CantGetTokenException(string message, Exception? innerException) : base(message, innerException)
    {
    }
}