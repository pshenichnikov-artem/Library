using Library.Core.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public class BookAddRequest
{
    [Required(ErrorMessage = "Title can't be blank")]
    [StringLength(100)]
    [CapitalizedAndMinLengthValidation(ErrorMessage = "Title must start with an uppercase letter")]
    public string? Title { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [CapitalizedAndMinLengthValidation(ErrorMessage = "Genre must start with an uppercase letter")]
    public string? Genre { get; set; }

    [Required(ErrorMessage = "Publication date can't be blank")]
    [RegularExpression(@"\d{4}-\d{2}-\d{2}", ErrorMessage = "Publication date must be in the format YYYY-MM-DD")]
    [StringToDateTypeValidation(ErrorMessage = "Publication date must be a valid date")]
    public string? PublicationDate { get; set; }

    [Required(ErrorMessage = "Author can't be blank")]
    public string? Author { get; set; }

    [DocxFileValidation(ErrorMessage = "The file must be a .docx file")]
    [FileOrOtherFileValidation(ErrorMessage = "At least one of DocxFile or PdfFile must be provided.")]
    public IFormFile? DocxFile { get; set; }

    [PdfFileValidation(ErrorMessage = "The file must be a .pdf file")]
    [FileOrOtherFileValidation(ErrorMessage = "At least one of DocxFile or PdfFile must be provided.")]
    public IFormFile? PdfFile { get; set; }

    [ImageFileValidation(ErrorMessage = "The image file must be a .jpg or .png file and cannot be empty.")]
    public IFormFile? ImageFile { get; set; }
}
