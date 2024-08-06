using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO.Account
{
    public class AccountUpdatePasswordRequest
    {
        [Required(ErrorMessage = "Old password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
