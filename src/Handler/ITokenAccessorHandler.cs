using ExtTokenManager.Model;

namespace ExtTokenManager.Handler;

public interface ITokenAccessorHandler
{
    Task<TokenWithRefresh> GetToken();

    Task<TokenWithRefresh> RefreshToken(string refreshToken);

    Task<TimeSpan> GetLifeTime();
}