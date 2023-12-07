namespace ExtTokenKeeper;

public interface ITokenKeeper
{
    void AddTokenFor<TService>(Func<string> getTokenFunc, Func<string>? refreshTokenFunc) where TService : class;
}