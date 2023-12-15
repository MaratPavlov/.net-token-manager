using ExtTokenManager.Handler;
using ExtTokenManager.Model;

namespace Demo;

public class TestTokenAccessorHandler : ITokenAccessorHandler
{
    public Task<TokenWithRefresh> GetToken()
    {
        return Task.FromResult(new TokenWithRefresh("first_token", "first_refresh"));
    }

    public Task<TokenWithRefresh> RefreshToken(string refreshToken)
    {
        return Task.FromResult(new TokenWithRefresh("after_refresh", "new_refresh"));
    }

    public Task<TimeSpan> GetLifeTime()
    {
        return Task.FromResult(TimeSpan.FromSeconds(10));
    }
}