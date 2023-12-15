using ExtTokenManager.Exceptions;
using ExtTokenManager.Handler;
using ExtTokenManager.Model;
using Microsoft.Extensions.DependencyInjection;

namespace ExtTokenManager;

/// <inheritdoc />
public class TokenAccessor : ITokenAccessor
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly List<ServiceTokenRecord> Configs = new();

    public TokenAccessor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public void AddTokenFor<TService>(
        Func<Task<TokenWithRefresh>> getTokenFunc,
        Func<string, Task<TokenWithRefresh>>? refreshTokenFunc,
        TimeSpan? lifetime) where TService : class
    {
        if (Configs.Any(x => x.ServiceType == typeof(TService)))
            throw new ServiceAlreadyRegisteredException(typeof(TService));

        Configs.Add(new ServiceTokenRecord(
            typeof(TService),
            GetTokenPrivate(getTokenFunc).Result,
            null,
            refreshTokenFunc,
            GetDueDate(lifetime),
            lifetime));
    }


    /// <inheritdoc />
    public void AddTokenFor<TService>(ExternalTokenConfiguration configuration) where TService : class
    {
        AddTokenFor<TService>(
            configuration.GetTokenFunc,
            configuration.RefreshTokenFunc,
            configuration.Lifetime);
    }

    public void AddTokenFor<TService>(Type handlerType) where TService : class
    {
        if (handlerType.IsAbstract || handlerType.IsClass == false || typeof(ITokenAccessorHandler).IsAssignableFrom(handlerType) == false)
            throw new ArgumentException($"{nameof(handlerType)} should be a class and implement the interface {nameof(ITokenAccessorHandler)}.");

        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService(handlerType) as ITokenAccessorHandler;
        if (handler is null)
            throw new ArgumentException($"Can`t get an instance of type {handlerType}");

        if (Configs.Any(x => x.ServiceType == typeof(TService)))
            throw new ServiceAlreadyRegisteredException(typeof(TService));

        var lifetime = handler.GetLifeTime().Result;
        Configs.Add(new ServiceTokenRecord(
            typeof(TService),
            handler.GetToken().Result,
            handlerType,
            null,
            GetDueDate(lifetime),
            lifetime));
    }

    /// <summary>
    /// Obtains the token for the service.
    /// </summary>
    /// <param name="forceUpdate">If refresh is required first.</param>
    /// <exception cref="ServiceWasNotRegisteredException">When an accessor has not been registered for this service.</exception>
    /// <exception cref="ArgumentNullException">When the token refresh function was not provided, but it is required.</exception>
    /// <exception cref="CantGetTokenException">When the token acquisition fails.</exception>
    public string GetTokenFor<TService>(bool forceUpdate = false) where TService : class
    {
        // get record 
        var config = Configs.FirstOrDefault(x => x.ServiceType == typeof(TService));
        if (config is null)
            throw new ServiceWasNotRegisteredException(typeof(TService));

        // check for date
        var now = DateTime.Now;
        if (config.DueDate > now && forceUpdate == false)
            return config.TokenWithRefresh.Token;

        TokenWithRefresh newToken;
        var newDueDate = now + config.Lifetime;

        if (config.TokenWithRefresh.RefreshToken is null)
            throw new ArgumentNullException(nameof(config.RefreshTokenFunc), "Refresh token can not be null");

        if (config.HandlerType is not null)
        {
            var scope = _serviceProvider.CreateScope();
            if (scope.ServiceProvider.GetRequiredService(config.HandlerType) is not ITokenAccessorHandler handler)
                throw new ArgumentException($"Can`t get an instance of type {config.HandlerType}");
            newToken = handler.RefreshToken(config.TokenWithRefresh.RefreshToken).Result;
        }
        else if (config.RefreshTokenFunc is null)
            throw new ArgumentNullException(nameof(config.RefreshTokenFunc), "Refresh function must be passed if lifetime exists");
        else
        {
            newToken = RefreshTokenPrivate(config.RefreshTokenFunc, config.TokenWithRefresh.RefreshToken).Result;
        }

        var newRecord = config with { TokenWithRefresh = newToken, DueDate = newDueDate };
        Configs.Remove(config);
        Configs.Add(newRecord);
        return newRecord.TokenWithRefresh.Token;
    }

    private static DateTime? GetDueDate(TimeSpan? lifetime)
        => lifetime.HasValue ? DateTime.Now + lifetime.Value : null;

    private static async Task<TokenWithRefresh> GetTokenPrivate(Func<Task<TokenWithRefresh>> getTokenFunc)
    {
        TokenWithRefresh token;
        try
        {
            token = await getTokenFunc();
        }
        catch (Exception e)
        {
            throw new CantGetTokenException("Can`t get initial token", e);
        }

        return token;
    }

    private static async Task<TokenWithRefresh> RefreshTokenPrivate(Func<string, Task<TokenWithRefresh>> getTokenFunc, string refreshToken)
    {
        TokenWithRefresh token;
        try
        {
            token = await getTokenFunc(refreshToken);
        }
        catch (Exception e)
        {
            throw new CantGetTokenException("Can`t refresh token", e);
        }

        return token;
    }
}