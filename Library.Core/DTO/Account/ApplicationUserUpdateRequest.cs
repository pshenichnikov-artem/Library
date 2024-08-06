using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO.Account
{
    public class ApplicationUserUpdateRequest
    {
        public Guid? UserId { get; set; }

        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string? LastName { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; } = string.Empty;
    }
}
