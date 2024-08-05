using System.ComponentModel.DataAnnotations;

namespace Library.Core.Validation
{
    public class StringLengthValidation : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public StringLengthValidation(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value as string;
            if (str != null)
            {
                if (str.Length < _minLength || str.Length > _maxLength)
                {
                    return new ValidationResult(ErrorMessage ?? $"The field must be between {_minLength} and {_maxLength} characters long.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
