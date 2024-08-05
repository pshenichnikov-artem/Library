

using Library.Core.DTO.Author;
using Library.Core.DTO.Book.BookFile;
using Library.Core.DTO.Book.BookImage;
using Library.Core.DTO.Comment;
using Library.Core.DTO.Rating;

namespace Library.Core.DTO.Book
{
    public class BookResponse
    {
        public Guid BookID { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public string? Genre { get; set; }

        public DateTime PublicationDate { get; set; }

        public IEnumerable<AuthorResponse>? Authors { get; set; }  // Added this line for multiple authors

        public IEnumerable<BookImageResponse>? BookImages { get; set; }

        public IEnumerable<BookFileResponse>? BookFiles { get; set; }

        public RatingResponse? Rating { get; set; }

        public IEnumerable<CommentResponse>? Comments { get; set; }

        public string? OwnerEmail { get; set; }
    }
}
