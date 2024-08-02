using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO
{
    public class BookAddRequest
    {
        [Required(ErrorMessage = "Title can't be blank")]
        [StringLength(100)]
        public string? Title { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        public string? Genre { get; set; }
        [Required(ErrorMessage = "Publication date can't be blank")]
        public string? PublicationDate { get; set; }
        [Required(ErrorMessage = "Author can't be blank")]
        public string? Author { get; set; }
    }
}
