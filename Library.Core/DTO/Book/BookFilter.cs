using Library.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO.Book
{
    public class BookFilter
    {
        [StringLengthValidation(1, 100, ErrorMessage = "Title must be between 1 and 100 characters long.")]
        public string? TitleOrAuthor { get; set; }

        [StringLengthValidation(0, 100, ErrorMessage = "Genre must be between 0 and 100 characters long.")]
        [GenreValidation(ErrorMessage = "Invalid genre.")]
        public List<string?>? Genre { get; set; }

        [DateValidation(invalidDataFormatOn: false, ErrorMessage = "Publication date must be a valid date between the year 1000 and today.")]
        public DateTime? PublicationDateFrom { get; set; }

        [DateValidation(invalidDataFormatOn: false, ErrorMessage = "Publication date must be a valid date between the year 1000 and today.")]
        [DateRangeValidation(nameof(PublicationDateFrom), ErrorMessage = "Publication date to must be greater than PublicationDateFrom.")]
        public DateTime? PublicationDateTo { get; set; }

        [Range(0,5)]
        public float? MinRating { get; set; }
    }
}
