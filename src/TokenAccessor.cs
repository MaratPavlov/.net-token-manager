using ExtTokenManager.Exceptions;
using ExtTokenManager.Model;

namespace ExtTokenManager;

/// <inheritdoc />
public class TokenAccessor : ITokenAccessor
{
    private static readonly List<ServiceTokenRecord> Storage = new();

    /// <inheritdoc />
    public void AddTokenFor<TService>(
        Func<Task<TokenWithRefresh>> getTokenFunc,
        Func<string, Task<TokenWithRefresh>>? refreshTokenFunc,
        TimeSpan? lifetime) where TService : class
    {
        if (Storage.Any(x => x.ServiceType == typeof(TService)))
            throw new ServiceAlreadyRegisteredException(typeof(TService));
        DateTime? dueDate = null;
        if (lifetime.HasValue)
            dueDate = DateTime.Now + lifetime.Value;

        var tokenWithRefresh = GetTokenPrivate(getTokenFunc).Result;
        var newRecord = new ServiceTokenRecord(
            typeof(TService),
            tokenWithRefresh,
            refreshTokenFunc,
            dueDate,
            lifetime);
        Storage.Add(newRecord);
    }

    /// <inheritdoc />
    public void AddTokenFor<TService>(ExternalTokenConfiguration configuration) where TService : class
    {
        AddTokenFor<TService>(
            configuration.GetTokenFunc,
            configuration.RefreshTokenFunc,
            configuration.Lifetime);
    }

    /// <summary>
    /// Obtains the token for the service.
    /// </summary>
    /// <param name="forceUpdate">If refresh is required first.</param>
    /// <exception cref="ServiceWasNotRegisteredException">When an accessor has not been registered for this service.</exception>
    /// <exception cref="ArgumentNullException">When the token refresh function was not provided, but it is required.</exception>
    /// <exception cref="CantGetTokenException">When the token acquisition fails.</exception>
    internal static string GetTokenFor<TService>(bool forceUpdate = false)
    {
        // get record
        var record = Storage.FirstOrDefault(x => x.ServiceType == typeof(TService));
        if (record is null)
            throw new ServiceWasNotRegisteredException(typeof(TService));

        // check for date
        var now = DateTime.Now;
        if (record.DueDate > now && forceUpdate == false)
            return record.TokenWithRefresh.Token;

        if (record.RefreshTokenFunc is null)
            throw new ArgumentNullException(nameof(record.RefreshTokenFunc), "Refresh function must be passed if lifetime exists");
        if (record.TokenWithRefresh.RefreshToken is null)
            throw new ArgumentNullException(nameof(record.RefreshTokenFunc), "Refresh token can not be null");

        var newToken = RefreshTokenPrivate(record.RefreshTokenFunc, record.TokenWithRefresh.RefreshToken).Result;
        var newRecord = record with { TokenWithRefresh = newToken };
        if (record.Lifetime.HasValue)
            newRecord = newRecord with { DueDate = now + record.Lifetime.Value };
        Storage.Remove(record);
        Storage.Add(newRecord);
        return newRecord.TokenWithRefresh.Token;
    }

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