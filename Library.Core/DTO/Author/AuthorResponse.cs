
using Library.Core.DTO.Author.AuthorImage;
using Library.Core.DTO.Book;

namespace Library.Core.DTO.Author
{
    public class AuthorResponse
    {
        public Guid AuthorID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Biography { get; set; }  // Optional field for author's biography

        public DateTime? DateOfBirth { get; set; }  // Optional field for author's date of birth

        public IEnumerable<AuthorImageResponse>? AuthorImages { get; set; }
        public IEnumerable<BookResponse> Books { get; set; }
    }
}
