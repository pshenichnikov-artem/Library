using System.ComponentModel.DataAnnotations;
using Library.Core.Domain.IdentityEntities;

namespace Library.Core.Domain.Entities
{
    public class BookRating
    {
        [Key]
        public Guid RatingID { get; set; }

        [Required]
        public Guid BookID { get; set; }
        public Book Book { get; set; } // Связь с книгой

        [Required]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; } // Рейтинг от 1 до 5

        public string? Review { get; set; } // Необязательный отзыв о книге
    }
}
