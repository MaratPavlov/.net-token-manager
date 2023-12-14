namespace ExtTokenManager;

/// <summary>
/// Provides the token.
/// </summary>
/// <typeparam name="TService">Service to which the token is provided.</typeparam>
public interface ITokenProvider<TService>
{
    /// <summary>
    /// Issuance.
    /// </summary>
    /// <returns>The token</returns>
    string GetToken();

    /// <summary>
    /// Issuance with force refresh.
    /// </summary>
    /// <returns>The token</returns>
    string RefreshNowAndGetToken();
}