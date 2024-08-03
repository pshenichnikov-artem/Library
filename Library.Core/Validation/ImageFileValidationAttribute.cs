using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class ImageFileValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if ((extension == ".jpg" || extension == ".png") && file.Length > 0)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage);
            }
        }
        return ValidationResult.Success;
    }
}
