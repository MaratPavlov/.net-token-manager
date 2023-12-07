namespace ExtTokenKeeper;

public interface ITokenProvider<TService>
{
    string GetToken();
}