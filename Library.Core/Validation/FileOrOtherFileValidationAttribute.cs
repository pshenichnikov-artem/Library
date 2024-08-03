using System.ComponentModel.DataAnnotations;

public class FileOrOtherFileValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var request = (BookAddRequest)validationContext.ObjectInstance;

        if (request.DocxFile == null && request.PdfFile == null)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
