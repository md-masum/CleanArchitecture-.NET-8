using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.DTOs.Account;

namespace CleanArchitecture.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<LoginResponse>> LoginAsync(LoginRequest request, string ipAddress);
        Task<Response<int>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<int>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<string>> ChangePassword(ChangePasswordRequest model);
        Task<Response<LoginResponse>> ExternalLoginAsync(string provider, string idToken, string ipAddress);
        Task<Response<LoginResponse>> ExternalRegisterAsync(string provider, string idToken, string ipAddress);
    }
}
