using Newtonsoft.Json;

namespace CleanArchitecture.Application.DTOs.Account
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string? JwtToken { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }
}
