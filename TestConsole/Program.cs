using System.Text;
using ExtTokenManager;
using ExtTokenManager.Model;
using Microsoft.Extensions.DependencyInjection;
using TestConsole;

var services = new ServiceCollection();
services
    .AddScoped<ITestService, TestService>()
    .UseExtTokenManager();
await using var provider = services.BuildServiceProvider();
await using var scope = provider.CreateAsyncScope();

var tokenAccessor = scope.ServiceProvider.GetRequiredService<ITokenAccessor>();

var refreshTokenBuilder = new StringBuilder();
refreshTokenBuilder.Append("init_refresh_token_");

tokenAccessor.AddTokenFor<ITestService>(
    // get token function
    () => new TokenWithRefresh(TokenGenerator.GenerateToken(), TokenGenerator.GenerateToken()),
    // refresh token function
    refreshToken =>
    {
        refreshTokenBuilder.Append("test");
        refreshTokenBuilder.Append(refreshToken);
        return new TokenWithRefresh(refreshTokenBuilder.ToString(), refreshToken);
    },
    // lifespan of token
    TimeSpan.FromSeconds(3));

var testService = scope.ServiceProvider.GetRequiredService<ITestService>();
while (true)
{
    Console.WriteLine($"{DateTime.Now} {testService.Test()}");
    Thread.Sleep(500);
}