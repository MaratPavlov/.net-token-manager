using ExtTokenKeeper.Exceptions;
using ExtTokenKeeper.Model;

namespace ExtTokenKeeper;

public class TokenKeeper : ITokenKeeper
{
    private static readonly List<ServiceTokenRecord> _storage = new();

    public void AddTokenFor<TService>(Func<string> getTokenFunc, Func<string>? refreshTokenFunc, TimeSpan? lifetime) where TService : class
    {
        if (_storage.Any(x => x.ServiceType == typeof(TService)))
            throw new ServiceAlreadyRegisteredException(typeof(TService));
        DateTime? dueDate = null;
        if (lifetime.HasValue)
            dueDate = DateTime.Now + lifetime.Value;

        var token = GetTokenInternal(getTokenFunc);
        var newRecord = new ServiceTokenRecord(typeof(TService), token, refreshTokenFunc, dueDate, lifetime);
        _storage.Add(newRecord);
    }

    internal static string GetTokenFor<TService>()
    {
        // get record
        var record = _storage.FirstOrDefault(x => x.ServiceType == typeof(TService));
        if (record is null)
            throw new ServiceWasNotRegisteredException(typeof(TService));

        // check for date
        var now = DateTime.Now;
        if (record.DueDate > now)
            return record.Token;

        if (record.RefreshTokenFunc is null)
            throw new ArgumentNullException(nameof(record.RefreshTokenFunc), "Refresh function must be passed if lifetime exists");

        var newToken = GetTokenInternal(record.RefreshTokenFunc);
        var newRecord = record with { Token = newToken };
        if (record.Lifetime.HasValue)
            newRecord = newRecord with { DueDate = now + record.Lifetime.Value };
        _storage.Remove(record);
        _storage.Add(newRecord);
        return newRecord.Token;
    }

    private static string GetTokenInternal(Func<string> getTokenFunc)
    {
        string token;
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
}