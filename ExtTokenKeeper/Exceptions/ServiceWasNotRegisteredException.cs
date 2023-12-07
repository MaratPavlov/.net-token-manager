namespace ExtTokenKeeper.Exceptions;

public class ServiceWasNotRegisteredException : Exception
{
    public ServiceWasNotRegisteredException(Type serviceType) : base($"Service {serviceType} was not registered in TokenKeeper")
    {
    }
}