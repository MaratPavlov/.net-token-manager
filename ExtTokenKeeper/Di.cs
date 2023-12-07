using Microsoft.Extensions.DependencyInjection;

namespace ExtTokenKeeper;

public static class Di
{
    /// <summary>
    /// Add external token keeper services to DI
    /// </summary>
    public static IServiceCollection UseExternalTokenKeeper(this IServiceCollection services)
        => services
            .AddScoped<ITokenKeeper, TokenKeeper>()
            .AddScoped(typeof(ITokenProvider<>), typeof(TokenProvider<>));
}