namespace TestConsole;

public class TokenGenerator
{
    public static string GenerateToken()
    {
        return Guid.NewGuid().ToString();
    }
}