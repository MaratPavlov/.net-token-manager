using ExtTokenKeeper;

namespace TestConsole;

public class TestService : ITestService
{
    private readonly ITokenProvider<TestService> _tokenProvider;

    public TestService(ITokenProvider<TestService> tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public string Test()
    {
        return _tokenProvider.GetToken();
    }
}