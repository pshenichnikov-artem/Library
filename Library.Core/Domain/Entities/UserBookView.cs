using Library.Core.Domain.IdentityEntities;
using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class UserBookView
    {
        [Key]
        public Guid UserBookViewID { get; set; }

        [Required]
        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public Guid BookID { get; set; }
        public Book Book { get; set; }

        [Required]
        public DateTime ViewDate { get; set; }
    }

}
