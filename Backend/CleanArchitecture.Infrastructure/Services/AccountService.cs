using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.DTOs.Account;
using CleanArchitecture.Application.DTOs.Email;
using CleanArchitecture.Application.Interfaces.Services;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Settings;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Infrastructure.Services
{
    public class AccountService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        SignInManager<ApplicationUser> signInManager,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor)
        : IAccountService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

        public async Task<Response<LoginResponse>> ExternalLoginAsync(string provider, string idToken, string ipAddress)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ApiException("Error loading external login information.");
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user == null)
                {
                    throw new ApiException("User not found after external login.");
                }
                JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
                LoginResponse response = new LoginResponse
                {
                    Id = user.Id,
                    JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Email = user.Email,
                    UserName = user.UserName
                };
                var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                response.Roles = rolesList.ToList();
                response.IsVerified = user.EmailConfirmed;
                var refreshToken = GenerateRefreshToken(ipAddress);
                response.RefreshToken = refreshToken.Token;
                return new Response<LoginResponse>(response, $"Authenticated {user.UserName}");
            }
            if (signInResult.IsLockedOut)
            {
                throw new ApiException("User account locked out.");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

                if (email == null)
                {
                    throw new ApiException("Email not found from external provider.");
                }

                var user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName ?? "", LastName = lastName ?? "", EmailConfirmed = true };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, nameof(Roles.Customer));
                        await signInManager.SignInAsync(user, isPersistent: false);
                        JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
                        LoginResponse response = new LoginResponse
                        {
                            Id = user.Id,
                            JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                            Email = user.Email,
                            UserName = user.UserName
                        };
                        var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                        response.Roles = rolesList.ToList();
                        response.IsVerified = user.EmailConfirmed;
                        var refreshToken = GenerateRefreshToken(ipAddress);
                        response.RefreshToken = refreshToken.Token;
                        return new Response<LoginResponse>(response, $"Authenticated {user.UserName}");
                    }
                }
                throw new ApiException($"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        public async Task<Response<LoginResponse>> ExternalRegisterAsync(string provider, string idToken, string ipAddress)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new ApiException("Error loading external login information during registration.");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            if (email == null)
            {
                throw new ApiException("Email not found from external provider during registration.");
            }

            var user = new ApplicationUser { UserName = email, Email = email, FirstName = firstName ?? "", LastName = lastName ?? "", EmailConfirmed = true };
            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                result = await userManager.AddLoginAsync(user, info);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, nameof(Roles.Customer));
                    await signInManager.SignInAsync(user, isPersistent: false);
                    JwtSecurityToken jwtSecurityToken = await GenerateJwtToken(user);
                    LoginResponse response = new LoginResponse
                    {
                        Id = user.Id,
                        JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        Email = user.Email,
                        UserName = user.UserName
                    };
                    var rolesList = await userManager.GetRolesAsync(user).ConfigureAwait(false);
                    response.Roles = rolesList.ToList();
                    response.IsVerified = user.EmailConfirmed;
                    var refreshToken = GenerateRefreshToken(ipAddress);
                    response.RefreshToken = refreshToken.Token;
                    return new Response<LoginResponse>(response, $"Authenticated {user.UserName}");
                }
            }
            throw new ApiException($"Error creating user during registration: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest request, string ipAddress)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new ApiException($"No accounts registered with email '{request.Email}'.");

            if (!user.EmailConfirmed)
                throw new ApiException($"Account not confirmed for '{request.Email}'.");

            var result = await signInManager.PasswordSignInAsync(user.UserName ?? string.Empty, request.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new ApiException("Invalid credentials.");

            var jwtToken = await GenerateJwtToken(user);

            var response = new LoginResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Roles = (await userManager.GetRolesAsync(user)).ToList(),
                IsVerified = user.EmailConfirmed,
                RefreshToken = GenerateRefreshToken(ipAddress).Token
            };

            return new Response<LoginResponse>(response, $"Authenticated as {user.UserName}");
        }


        public async Task<Response<int>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            var userWithSameEmail = await userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, nameof(Roles.Customer));
                    var verificationUri = await SendVerificationEmail(user, origin);
                    //TODO: Attach Email Service here and configure it via appsettings
                    await emailService.SendAsync(new EmailRequest() { From = "superstarshoyon@gmail.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                    return new Response<int>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }
        }

        public async Task<Response<int>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    return new Response<int>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/login endpoint.");
                }
            }

            throw new ApiException($"An error occured while confirming {user?.Email}.");
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await userManager.FindByEmailAsync(model.Email);
            if (account is null) return;
            var passwordResetUri = await SendResetPasswordEmail(account, origin);
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {passwordResetUri}",
                To = model.Email,
                Subject = "Reset Password",
            };
            await emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await userManager.FindByEmailAsync(model.Email);
            if (account is null) throw new ApiException($"No Accounts Registered with {model.Email}.");

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            var result = await userManager.ResetPasswordAsync(account, token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Reseted.");
            }

            throw new ApiException($"Error occured while reseting the password.");
        }

        public async Task<Response<string>> ChangePassword(ChangePasswordRequest model)
        {
            var account = await userManager.FindByEmailAsync(model.Email);
            if (account is null) throw new ApiException($"No Accounts Registered with {model.Email}.");

            var checkOldPassword = await signInManager.PasswordSignInAsync(account.UserName ?? string.Empty, model.OldPassword, false, lockoutOnFailure: false);
            if (!checkOldPassword.Succeeded)
            {
                throw new ApiException($"Invalid Credentials for '{account.Email}'.");
            }

            var result = await userManager.ChangePasswordAsync(account, model.OldPassword, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(account.Email ?? string.Empty, "Password changed");
            }
            throw new ApiException($"Error occured while change the password.");
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            string ipAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty;

            var claims = new[]
                {
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                    new Claim("ip", ipAddress)
                }
                .Union(userClaims)
                .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[40];
            rng.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendResetPasswordEmail(ApplicationUser user, string origin)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/reset-password/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "email", user.Email ?? string.Empty);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);
            //Email Service Call Here
            return verificationUri;
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
    }

}
