namespace ExtTokenKeeper;

public class TokenKeeper : ITokenKeeper
{
    public void AddTokenFor<TService>(Func<string> getTokenFunc, Func<string>? refreshTokenFunc) where TService : class
    {
    }

    internal string GetTokenFor()
    {
        return "from service";
    }
}