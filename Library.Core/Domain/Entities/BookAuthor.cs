using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class BookAuthor
    {
        [Key]
        public Guid BookAuthorID { get; set; }

        public Guid BookID { get; set; }
        public Book Book { get; set; } = default!;

        public Guid AuthorID { get; set; }
        public Author Author { get; set; } = default!;
    }
}
