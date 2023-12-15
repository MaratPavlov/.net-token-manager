using ExtTokenManager.Exceptions;

namespace ExtTokenManager;

/// <summary>
/// Provides the token.
/// </summary>
/// <typeparam name="TService">Service to which the token is provided.</typeparam>
public interface ITokenProvider<TService>
{
    /// <summary>
    /// Obtains the token for the service.
    /// </summary>
    /// <exception cref="ServiceWasNotRegisteredException">When an accessor has not been registered for this service.</exception>
    /// <exception cref="ArgumentNullException">When the token refresh function was not provided, but it is required.</exception>
    /// <exception cref="CantGetTokenException">When the token acquisition fails.</exception>
    string GetToken();

    /// <summary>
    /// Obtains the token, but through refreshing.
    /// </summary>
    /// <exception cref="ServiceWasNotRegisteredException">When an accessor has not been registered for this service.</exception>
    /// <exception cref="ArgumentNullException">When the token refresh function was not provided, but it is required.</exception>
    /// <exception cref="CantGetTokenException">When the token acquisition fails.</exception>
    string RefreshNowAndGetToken();
}