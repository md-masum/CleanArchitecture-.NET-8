using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.DTOs.Account
{
    public class ChangePasswordRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string OldPassword { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
