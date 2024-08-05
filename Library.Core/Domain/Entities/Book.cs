

using Library.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class Book
    {
        [Key]
        public Guid BookID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Genre { get; set; }

        public DateTime? PublicationDate { get; set; }

        [Required]
        public Guid OwnerID { get; set; }
        public ApplicationUser Owner { get; set; } = default!;

        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<BookImage> BookImages { get; set; } = new List<BookImage>();
        public ICollection<BookFile> BookFiles { get; set; } = new List<BookFile>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserBookView> UserViews { get; set; }
        public Rating? Rating { get; set; }
    }
}
