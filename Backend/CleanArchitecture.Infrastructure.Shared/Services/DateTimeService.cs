using CleanArchitecture.Application.Interfaces.Services;

namespace CleanArchitecture.Infrastructure.Shared.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public void Dispose()
        {
        }
    }
}
