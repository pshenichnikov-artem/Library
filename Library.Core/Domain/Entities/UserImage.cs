using System.ComponentModel.DataAnnotations;
using Library.Core.Domain.IdentityEntities;

namespace Library.Core.Domain.Entities
{
    public class UserImage
    {
        [Key]
        public Guid UserImageID { get; set; }

        [Required]
        public Guid UserID { get; set; }
        public ApplicationUser User { get; set; } = default!;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;
    }
}
