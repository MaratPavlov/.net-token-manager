namespace ExtTokenManager.Model;

/// <summary>
/// Token acquisition configuration.
/// </summary>
/// <param name="GetTokenFunc">Token acquisition function.</param>
/// <param name="RefreshTokenFunc">Token refresh function.</param>
/// <param name="Lifetime">Token lifespan.</param>
public record ExternalTokenConfiguration(
    Func<TokenWithRefresh> GetTokenFunc,
    Func<string, TokenWithRefresh>? RefreshTokenFunc,
    TimeSpan? Lifetime);