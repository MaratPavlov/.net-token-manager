using ExtTokenManager;

namespace Demo;

public class TestHandlerService : ITestHandlerService
{
    private readonly ITokenProvider<ITestHandlerService> _tokenProvider;

    public TestHandlerService(ITokenProvider<ITestHandlerService> tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public string Test()
    {
        return _tokenProvider.GetToken();
    }
}