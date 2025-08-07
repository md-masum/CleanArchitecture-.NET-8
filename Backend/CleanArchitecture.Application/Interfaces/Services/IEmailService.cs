using CleanArchitecture.Application.DTOs.Email;

namespace CleanArchitecture.Application.Interfaces.Services
{
    public interface IEmailService : IDisposable
    {
        Task SendAsync(EmailRequest request);
    }
}
