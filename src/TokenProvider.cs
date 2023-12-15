namespace ExtTokenManager;

/// <inheritdoc />
public class TokenProvider<TService> : ITokenProvider<TService> where TService : class
{
    private readonly ITokenAccessor _tokenAccessor;

    public TokenProvider(ITokenAccessor tokenAccessor)
    {
        _tokenAccessor = tokenAccessor;
    }

    /// <inheritdoc />
    public string GetToken()
    {
        return _tokenAccessor.GetTokenFor<TService>();
    }

    /// <inheritdoc />
    public string RefreshNowAndGetToken()
    {
        return _tokenAccessor.GetTokenFor<TService>(true);
    }
}