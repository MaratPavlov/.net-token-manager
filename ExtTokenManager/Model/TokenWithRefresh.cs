namespace ExtTokenManager.Model;

/// <summary>
/// Tokens.
/// </summary>
/// <param name="Token">Access token.</param>
/// <param name="RefreshToken">Refresh token.</param>
public record TokenWithRefresh(string Token, string? RefreshToken);