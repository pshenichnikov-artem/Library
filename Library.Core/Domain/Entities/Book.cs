

using System.ComponentModel.DataAnnotations;

namespace Library.Core.Domain.Entities
{
    public class Book
    {
        [Key]
        public Guid BookID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string? Author { get; set; }
        public IEnumerable<BookFile> BookFile { get; set; }
        public Guid CoverImageID { get; set; }
        public Image Cover {  get; set; }
    }
}
