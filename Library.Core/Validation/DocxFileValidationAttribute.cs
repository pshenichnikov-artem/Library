using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Validation
{
    public class DocxFileValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file != null && file.Length > 0 && Path.GetExtension(file.FileName).ToLowerInvariant() == ".docx")
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
