using ExtTokenManager;
using ExtTokenManager.Model;
using Microsoft.Extensions.DependencyInjection;
using Demo;

var services = new ServiceCollection();
services
    .AddScoped<ITestService, TestService>()
    .AddScoped<ITestHandlerService, TestHandlerService>()
    .UseExtTokenManager()
    .AddTokenAccessorHandler(typeof(TestTokenAccessorHandler));
await using var provider = services.BuildServiceProvider();
await using var scope = provider.CreateAsyncScope();

var tokenAccessor = scope.ServiceProvider.GetRequiredService<ITokenAccessor>();

// config
tokenAccessor.AddTokenFor<ITestService>(
    // get token function
    () => Task.FromResult(new TokenWithRefresh(TokenGenerator.GenerateToken(), TokenGenerator.GenerateToken())),
    // refresh token function
    refreshToken => { return Task.FromResult(new TokenWithRefresh(TokenGenerator.GenerateToken(), TokenGenerator.GenerateToken())); },
    // lifespan of token
    TimeSpan.FromSeconds(10));

// handler
tokenAccessor.AddTokenFor<ITestHandlerService>(typeof(TestTokenAccessorHandler));

var testService = scope.ServiceProvider.GetRequiredService<ITestService>();
var testHandlerService = scope.ServiceProvider.GetRequiredService<ITestHandlerService>();
while (true)
{
    Console.WriteLine($"TEST        : {DateTime.Now} {testService.Test()}");
    Console.WriteLine($"TEST_HANDLER: {DateTime.Now} {testHandlerService.Test()}");
    Thread.Sleep(500);
}