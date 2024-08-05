using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Core.Domain.IdentityEntities;

namespace Library.Core.Domain.Entities
{
    public class Comment
    {
        [Key]
        public Guid CommentID { get; set; }

        [Required]
        public Guid BookID { get; set; }
        public Book Book { get; set; } = default!;

        [Required]
        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; } = default!;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
