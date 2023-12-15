using Microsoft.Extensions.DependencyInjection;

namespace ExtTokenManager;

public static class Di
{
    /// <summary>
    /// Add external token manager services to DI
    /// </summary>
    public static IServiceCollection UseExtTokenManager(this IServiceCollection services)
        => services
            .AddSingleton<ITokenAccessor, TokenAccessor>()
            .AddScoped(typeof(ITokenProvider<>), typeof(TokenProvider<>));

    public static IServiceCollection AddTokenAccessorHandler(this IServiceCollection services, Type handlerType)
        => services.AddScoped(handlerType);
}