
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Validation
{
    public class CapitalizedValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value as string;
            if (str != null && !string.IsNullOrEmpty(str) && !char.IsUpper(str[0]))
            {
                return new ValidationResult(ErrorMessage ?? "The field must start with an uppercase letter.");
            }
            return ValidationResult.Success;
        }
    }
}
