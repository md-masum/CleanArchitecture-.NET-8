using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Application.DTOs.Account;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public required string LastName { get; set; }
        public List<RefreshToken>? RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
