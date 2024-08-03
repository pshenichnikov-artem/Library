using System.ComponentModel.DataAnnotations;

public class CapitalizedAndMinLengthValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var genre = value as string;

        if (string.IsNullOrEmpty(genre))
        {
            return ValidationResult.Success;
        }

        if (genre.Length < 2)
        {
            return new ValidationResult("Genre must be at least 2 characters long");
        }

        if (!char.IsUpper(genre[0]))
        {
            return new ValidationResult(ErrorMessage ?? "Must start with an uppercase letter");
        }

        return ValidationResult.Success;
    }
}
