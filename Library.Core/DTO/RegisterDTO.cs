using Library.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "First name can't be blank")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "First name must contains onle a-z, A-Z")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name can't be blank")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Last name must contains onle a-z, A-Z")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; }
    }
}
