namespace CleanArchitecture.Application.Interfaces.Services
{
    public interface IDateTimeService : IDisposable
    {
        DateTime Now { get; }
    }
}