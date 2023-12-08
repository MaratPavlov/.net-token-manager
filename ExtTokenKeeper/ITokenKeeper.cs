using ExtTokenKeeper.Model;

namespace ExtTokenKeeper;

public interface ITokenKeeper
{
    void AddTokenFor<TService>(Func<TokenWithRefresh> getTokenFunc, Func<string, TokenWithRefresh>? refreshTokenFunc, TimeSpan? lifetime)
        where TService : class;
}