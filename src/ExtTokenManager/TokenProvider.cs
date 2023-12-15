namespace ExtTokenManager;

/// <inheritdoc />
public class TokenProvider<TService> : ITokenProvider<TService> where TService : class
{
    /// <inheritdoc />
    public string GetToken()
    {
        return TokenAccessor.GetTokenFor<TService>();
    }

    public string RefreshNowAndGetToken()
    {
        return TokenAccessor.GetTokenFor<TService>(true);
    }
}