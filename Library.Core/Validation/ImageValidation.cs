using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Validation
{
    public class ImageValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length == 0 || !(file.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                           file.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
