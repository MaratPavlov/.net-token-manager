<div align="center">

[![NuGet](https://img.shields.io/nuget/v/ExtTokenManager.svg)](https://www.nuget.org/packages/ExtTokenManager)

[![Build](https://github.com/maratpavlov/exttokenmanager/actions/workflows/build.yml/badge.svg)](https://github.com/maratpavlov/exttokenmanager/actions/workflows/build.yml) [![publish to nuget](https://github.com/maratpavlov/exttokenmanager/actions/workflows/deploy.yml/badge.svg)](https://github.com/maratpavlov/exttokenmanager/actions/workflows/deploy.yml)

# A simple method to store, retrieve, and refresh tokens for external services.

`dotnet add package ExtTokenManager`

</div>

# Usage

## Startup
```csharp
using ExtTokenManager;

// ...

services.UseExtTokenManager();
```

## Initialize
```csharp
var tokentAccessor = serviceProvider.GetRequiredService<ITokenAccessor>();
tokenAccessor.AddTokenFor<ITestService>(
    // get token function
    () =>
    {
        // get token first time logic...
        new TokenWithRefresh(token, refreshToken);
    },
    // refresh token function
    refreshToken =>
    {
        // refresh token logic using refreshToken input
        return new TokenWithRefresh(newToken, newRefreshToken);
    },
    // lifespan of token
    TimeSpan.FromMinutes(60));
```

## Use in service and get token.
You don't need to worry about the mechanism of its acquisition.

### Get ITokenProvider via DI. 

```charp
private readonly ITokenProvider<ITestService> _tokenProvider;

public TestService(ITokenProvider<ITestService> tokenProvider)
{
    _tokenProvider = tokenProvider;
}
```

### Get the token!
```csharp
var token = _tokenProvider.GetToken();
```

### Sometimes you want to force refresh
```csharp
var token = _tokenProvider.RefreshNowAndGetToken();
```

See full [Demo project](https://github.com/MaratPavlov/ExtTokenManager/tree/main/Demo)

# Contribution

If you have any questions, comments, or suggestions, please open an issue or create a pull request 🙂

# License

This project is licensed under the terms of the [MIT](https://github.com/MaratPavlov/ExtTokenManager/blob/main/LICENSE) license.
