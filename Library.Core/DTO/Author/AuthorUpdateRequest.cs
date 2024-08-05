using Library.Core.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO.Author
{
    public class AuthorUpdateRequest
    {
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Biography must be between 3 and 1000 characters.")]
        [CapitalizedValidation(ErrorMessage = "Biography must start with a capital letter.")]
        public string? Biography { get; set; }
        [ImageValidation(ErrorMessage = "The provided image is not valid.")]
        public IFormFile? Image { get; set; }
    }
}
