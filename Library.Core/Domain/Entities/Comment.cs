using System.ComponentModel.DataAnnotations;
using Library.Core.Domain.IdentityEntities;

namespace Library.Core.Domain.Entities
{
    public class Comment
    {
        [Key]
        public Guid CommentID { get; set; }


        public Guid BookID { get; set; }
        public Book Book { get; set; } = default!;

        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; } = default!;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
