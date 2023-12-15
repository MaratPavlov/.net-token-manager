namespace Demo;

public class TokenGenerator
{
    public static string GenerateToken()
    {
        return Guid.NewGuid().ToString();
    }
}