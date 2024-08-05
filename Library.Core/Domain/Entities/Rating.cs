using System.ComponentModel.DataAnnotations;
using Library.Core.Domain.IdentityEntities;

namespace Library.Core.Domain.Entities
{
    public class Rating
    {
        [Key]
        public Guid RatingID { get; set; }

        [Required]
        public Guid BookID { get; set; }
        public Book Book { get; set; } = default!;

        [Required]
        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; } = default!;

        [Range(1, 5)]
        public int Value { get; set; }
    }
}
