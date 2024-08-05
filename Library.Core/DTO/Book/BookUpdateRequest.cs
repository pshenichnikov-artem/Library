using Library.Core.Validation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO.Book
{
    public class BookUpdateRequest
    {
        [Required(ErrorMessage = "Book ID is required.")]
        public Guid BookID { get; set; }

        [Required(ErrorMessage = "Title can't be blank.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters long.")]
        [CapitalizedValidation(ErrorMessage = "Title must start with an uppercase letter.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [StringLength(100, ErrorMessage = "Genre must be between 0 and 100 characters long.")]
        [GenreValidation]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "Publication date can't be blank.")]
        [DateValidation(ErrorMessage = "Publication date must be a valid date between the year 1000 and today.")]
        public DateTime? PublicationDate { get; set; }

        public ICollection<Guid>? AuthorIds { get; set; }

        [FileDocxValidation(ErrorMessage = "FileDocx must be a valid .docx file.")]
        public IFormFile? FileDocx { get; set; }

        [FileDocxOrPdfExistValidation("FileDocx", "FilePdf", ErrorMessage = "At least one of FileDocx or FilePdf must be provided.")]
        [FilePdfValidation(ErrorMessage = "FilePdf must be a valid .pdf file.")]
        public IFormFile? FilePdf { get; set; }

        [ImageValidation(ErrorMessage = "Image must be a .jpg or .png file and cannot be empty.")]
        public IFormFile? Image { get; set; }
    }
}
