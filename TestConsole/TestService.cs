using ExtTokenManager;

namespace TestConsole;

public class TestService : ITestService
{
    private readonly ITokenProvider<ITestService> _tokenProvider;

    public TestService(ITokenProvider<ITestService> tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public string Test()
    {
        _tokenProvider.RefreshNowAndGetToken();
        return _tokenProvider.GetToken();
    }
}