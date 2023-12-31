namespace ExtTokenManager.Model;

internal record ServiceTokenRecord(
    Type ServiceType,
    TokenWithRefresh TokenWithRefresh,
    Func<string, Task<TokenWithRefresh>>? RefreshTokenFunc,
    DateTime? DueDate,
    TimeSpan? Lifetime);