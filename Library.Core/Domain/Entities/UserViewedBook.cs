using Library.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Domain.Entities
{
    public class UserViewedBook
    {
        [Required]
        public Guid UserID { get; set; }
        [Required]
        public Guid BookID { get; set; }

        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; } = default!;

        [ForeignKey(nameof(BookID))]
        public Book Book { get; set; } = default!;

        [Required]
        public DateTime ViewDate { get; set; } = DateTime.Now;
    }
}
