using Library.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Library.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<UserImage> UserImages { get; set; } = new HashSet<UserImage>();
        public virtual ICollection<UserBookView> UserBookViews { get; set; } = new List<UserBookView>();
    }
}
