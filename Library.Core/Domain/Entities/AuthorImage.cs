using System.ComponentModel.DataAnnotations;


namespace Library.Core.Domain.Entities
{
    public class AuthorImage
    {
        [Key]
        public Guid AuthorImageID { get; set; }

        [Required]
        public Guid AuthorID { get; set; }
        public Author Author { get; set; } = default!;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;
    }
}
