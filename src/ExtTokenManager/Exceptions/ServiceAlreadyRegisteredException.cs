namespace ExtTokenManager.Exceptions;

public class ServiceAlreadyRegisteredException : Exception
{
    public ServiceAlreadyRegisteredException(Type serviceType) : base($"Service {serviceType} is already registered in TokenKeeper")
    {
    }
}