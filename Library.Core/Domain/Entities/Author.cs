using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class Author
    {
        [Key]
        public Guid AuthorID { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<AuthorImage> AuthorImages { get; set; } = new List<AuthorImage>();
    }
}
