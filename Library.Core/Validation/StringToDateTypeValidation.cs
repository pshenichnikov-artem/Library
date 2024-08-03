using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Library.Core.Validation
{
    public class StringToDateTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string dateStr = value.ToString()!;

            // Проверка на правильность формата даты
            if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return new ValidationResult(ErrorMessage ?? "Invalid date format.");
            }

            // Проверка на диапазон даты
            DateTime minDate = new DateTime(1000, 1, 1);
            DateTime maxDate = DateTime.Today;

            if (parsedDate < minDate || parsedDate > maxDate)
            {
                return new ValidationResult(ErrorMessage ?? "Date must be between January 1, 1000 and today's date.");
            }

            return ValidationResult.Success;
        }
    }
}
