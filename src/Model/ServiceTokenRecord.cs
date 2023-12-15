namespace ExtTokenManager.Model;

internal record ServiceTokenRecord(
    Type ServiceType,
    TokenWithRefresh TokenWithRefresh,
    Type? HandlerType,
    Func<string, Task<TokenWithRefresh>>? RefreshTokenFunc,
    DateTime? DueDate,
    TimeSpan? Lifetime);