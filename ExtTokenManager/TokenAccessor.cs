using ExtTokenManager.Exceptions;
using ExtTokenManager.Model;

namespace ExtTokenManager;

/// <inheritdoc />
public class TokenAccessor : ITokenAccessor
{
    private static readonly List<ServiceTokenRecord> Storage = new();

    /// <inheritdoc />
    public void AddTokenFor<TService>(
        Func<TokenWithRefresh> getTokenFunc,
        Func<string, TokenWithRefresh>? refreshTokenFunc,
        TimeSpan? lifetime) where TService : class
    {
        if (Storage.Any(x => x.ServiceType == typeof(TService)))
            throw new ServiceAlreadyRegisteredException(typeof(TService));
        DateTime? dueDate = null;
        if (lifetime.HasValue)
            dueDate = DateTime.Now + lifetime.Value;

        var tokenWithRefresh = GetTokenInternal(getTokenFunc);
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

    internal static string GetTokenFor<TService>()
    {
        // get record
        var record = Storage.FirstOrDefault(x => x.ServiceType == typeof(TService));
        if (record is null)
            throw new ServiceWasNotRegisteredException(typeof(TService));

        // check for date
        var now = DateTime.Now;
        if (record.DueDate > now)
            return record.TokenWithRefresh.Token;

        if (record.RefreshTokenFunc is null)
            throw new ArgumentNullException(nameof(record.RefreshTokenFunc), "Refresh function must be passed if lifetime exists");
        if (record.TokenWithRefresh.RefreshToken is null)
            throw new ArgumentNullException(nameof(record.RefreshTokenFunc), "Refresh token can not be null");

        var newToken = RefreshTokenInternal(record.RefreshTokenFunc, record.TokenWithRefresh.RefreshToken);
        var newRecord = record with { TokenWithRefresh = newToken };
        if (record.Lifetime.HasValue)
            newRecord = newRecord with { DueDate = now + record.Lifetime.Value };
        Storage.Remove(record);
        Storage.Add(newRecord);
        return newRecord.TokenWithRefresh.Token;
    }

    private static TokenWithRefresh GetTokenInternal(Func<TokenWithRefresh> getTokenFunc)
    {
        TokenWithRefresh token;
        try
        {
            token = getTokenFunc();
        }
        catch (Exception e)
        {
            throw new CantGetTokenException("Can`t get initial token", e);
        }

        return token;
    }

    private static TokenWithRefresh RefreshTokenInternal(Func<string, TokenWithRefresh> getTokenFunc, string refreshToken)
    {
        TokenWithRefresh token;
        try
        {
            token = getTokenFunc(refreshToken);
        }
        catch (Exception e)
        {
            throw new CantGetTokenException("Can`t refresh token", e);
        }

        return token;
    }
}