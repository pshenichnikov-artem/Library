

using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class BookFile
    {
        [Key]
        public Guid BookFileID { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }

        public Guid BookID { get; set; }
        public Book? Book { get; set; }
    }
}
