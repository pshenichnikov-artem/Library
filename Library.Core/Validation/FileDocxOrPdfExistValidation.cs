using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Validation
{
    public class FileDocxOrPdfExistValidation : ValidationAttribute
    {
        private readonly string _fileDocxPropertyName;
        private readonly string _filePdfPropertyName;

        public FileDocxOrPdfExistValidation(string fileDocxPropertyName, string filePdfPropertyName)
        {
            _fileDocxPropertyName = fileDocxPropertyName;
            _filePdfPropertyName = filePdfPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var fileDocxProperty = validationContext.ObjectType.GetProperty(_fileDocxPropertyName);
            var filePdfProperty = validationContext.ObjectType.GetProperty(_filePdfPropertyName);

            if (fileDocxProperty == null || filePdfProperty == null)
            {
                throw new ArgumentNullException("FileRequiredValidation");
            }

            var fileDocxValue = fileDocxProperty.GetValue(validationContext.ObjectInstance) as IFormFile;
            var filePdfValue = filePdfProperty.GetValue(validationContext.ObjectInstance) as IFormFile;

            if (fileDocxValue == null && filePdfValue == null)
            {
                return new ValidationResult(ErrorMessage ?? "At least one of FileDocx or FilePdf must be provided.");
            }

            return ValidationResult.Success;
        }
    }
}
