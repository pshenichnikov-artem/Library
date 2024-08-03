using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class PdfFileValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file != null && file.Length > 0 && Path.GetExtension(file.FileName).ToLowerInvariant() == ".pdf")
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}
