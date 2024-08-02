using Library.Core.Domain.Entities;

namespace Library.Core.DTO
{
    public class BookResponse
    {
        public Guid BookID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public string? PublicationDate { get; set; }
        public string? Author { get; set; }
        public Guid? BookFileDocx { get; set; }
        public Guid? BookFilePdf { get; set; }
        public Guid? CoverImageID { get; set; }
    }
}
