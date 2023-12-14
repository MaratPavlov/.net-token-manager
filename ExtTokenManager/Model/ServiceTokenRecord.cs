namespace ExtTokenManager.Model;

internal record ServiceTokenRecord(
    Type ServiceType,
    TokenWithRefresh TokenWithRefresh,
    Func<string, TokenWithRefresh>? RefreshTokenFunc,
    DateTime? DueDate,
    TimeSpan? Lifetime);