namespace ExtTokenManager.Model;

/// <summary>
/// Token acquisition configuration.
/// </summary>
/// <param name="GetTokenFunc">Token acquisition function.</param>
/// <param name="RefreshTokenFunc">Token refresh function.</param>
/// <param name="Lifetime">Token lifespan.</param>
public record ExternalTokenConfiguration(
    Func<Task<TokenWithRefresh>> GetTokenFunc,
    Func<string, Task<TokenWithRefresh>>? RefreshTokenFunc,
    TimeSpan? Lifetime);