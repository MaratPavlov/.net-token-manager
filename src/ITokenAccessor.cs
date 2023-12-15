using ExtTokenManager.Model;

namespace ExtTokenManager;

/// <summary>
/// Allows configuring the token acquisition and refresh mechanism.
/// </summary>
public interface ITokenAccessor
{
    /// <summary>
    /// Adds token acquisition configuration for the service.
    /// </summary>
    /// <param name="getTokenFunc">Token acquisition function.</param>
    /// <param name="refreshTokenFunc">Token refresh function.</param>
    /// <param name="lifetime">Token lifespan.</param>
    /// <typeparam name="TService">Service utilizing this token.</typeparam>
    void AddTokenFor<TService>(Func<Task<TokenWithRefresh>> getTokenFunc, Func<string, Task<TokenWithRefresh>>? refreshTokenFunc, TimeSpan? lifetime)
        where TService : class;

    /// <summary>
    /// Adds token acquisition configuration for the service.
    /// </summary>
    /// <param name="configuration">Token acquisition and refresh configuration.</param>
    /// <typeparam name="TService">Service utilizing this token.</typeparam>
    void AddTokenFor<TService>(ExternalTokenConfiguration configuration) where TService : class;
}