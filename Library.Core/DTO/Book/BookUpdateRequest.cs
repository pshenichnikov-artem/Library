using Microsoft.AspNetCore.Http;

namespace Library.Core.DTO.Book
{
    public class BookUpdateRequest
    {
        public Guid BookID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public DateTime? PublicationDate { get; set; }
        public ICollection<Guid>? AuthorIds { get; set; }
        public ICollection<IFormFile>? Files { get; set; }
        public IFormFile? Image { get; set; }
    }
}
