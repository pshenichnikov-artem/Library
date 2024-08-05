using System.ComponentModel.DataAnnotations;

namespace Library.Core.Validation
{
    public class DateRangeValidationAttribute : ValidationAttribute
    {
        private readonly string _startDateProperty;

        public DateRangeValidationAttribute(string startDateProperty)
        {
            _startDateProperty = startDateProperty;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var endDate = (DateTime?)value;

            var startDatePropertyInfo = validationContext.ObjectType.GetProperty(_startDateProperty);
            if (startDatePropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_startDateProperty}");
            }

            var startDate = (DateTime?)startDatePropertyInfo.GetValue(validationContext.ObjectInstance);

            if (endDate != null && startDate != null && endDate < startDate)
            {
                return new ValidationResult(ErrorMessage ?? "End date must be greater than start date.");
            }

            return ValidationResult.Success;
        }
    }
}
