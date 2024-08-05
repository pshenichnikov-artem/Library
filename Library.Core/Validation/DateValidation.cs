using Library.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Validation
{
    public class DateValidation : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate = DateTime.Today;
        private readonly bool _invalidDataFormatOn;

        public DateValidation(DateTime minDate, bool invalidDataFormatOn = true)
        {
            _maxDate = minDate;
            _invalidDataFormatOn = invalidDataFormatOn;
        }

        public DateValidation(bool invalidDataFormatOn = true)
        {
            _minDate = new DateTime(1000, 1, 1);
            _invalidDataFormatOn = invalidDataFormatOn;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date < _minDate || date > _maxDate)
                {
                    return new ValidationResult(ErrorMessage ?? $"The publication date must be between {_minDate.ToShortDateString()} and {_maxDate.ToShortDateString()}.");
                }
            }
            else if(_invalidDataFormatOn)
            {
                return new ValidationResult("Invalid date format.");
            }

            return ValidationResult.Success;
        }
    }
}
