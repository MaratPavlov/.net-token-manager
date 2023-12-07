namespace ExtTokenKeeper.Model;

public record ServiceTokenRecord(
    Type ServiceType,
    string Token,
    Func<string>? RefreshTokenFunc,
    DateTime? DueDate,
    TimeSpan? Lifetime);