using Library.Core.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Library.Core.DTO.Author
{
    public class AuthorAddRequest
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First name must be between 3 and 100 characters.")]
        [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = "Error symbol int first name")]
        [CapitalizedValidation(ErrorMessage = "First name must start with a capital letter.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last name must be between 3 and 100 characters.")]
        [RegularExpression("^[A-Z][a-z]+$", ErrorMessage = "Error symbol in last name")]
        [CapitalizedValidation(ErrorMessage = "Last name must start with a capital letter.")]
        public string? LastName { get; set; }

        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Biography must be between 3 and 1000 characters.")]
        [CapitalizedValidation(ErrorMessage = "Biography must start with a capital letter.")]
        public string? Biography { get; set; }

        [DateValidation(ErrorMessage = "Date of birth must be a valid date.")]
        [Required]
        public DateTime? DateOfBirth { get; set; }

        [ImageValidation(ErrorMessage = "The provided image is not valid.")]
        public IFormFile? Image { get; set; }
    }
}
