namespace ExtTokenKeeper;

public class TokenProvider<TService> : ITokenProvider<TService> where TService : class
{
    public string GetToken()
    {
        return TokenKeeper.GetTokenFor<TService>();
    }
}