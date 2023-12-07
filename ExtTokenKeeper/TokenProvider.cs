namespace ExtTokenKeeper;

public class TokenProvider<TService> : ITokenProvider<TService> where TService : class
{
    private readonly TokenKeeper _tokenKeeper;

    public TokenProvider()
    {
        _tokenKeeper = new TokenKeeper();
    }

    public string GetToken()
    {
        return _tokenKeeper.GetTokenFor();
    }
}