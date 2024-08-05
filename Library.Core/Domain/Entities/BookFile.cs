

using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class BookFile
    {
        [Key]
        public Guid BookFileID { get; set; }

        [Required]
        public Guid BookID { get; set; }
        public Book Book { get; set; } = default!;

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string FileType { get; set; } = string.Empty; // e.g., "pdf" or "docx"
    }
}
