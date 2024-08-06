using Library.Core.Domain.IdentityEntities;
using Library.Core.DTO.Book;

namespace Library.Core.DTO
{
    public class UserBookViewResponse
    {
        public ApplicationUser User { get; set; }
        public IDictionary<DateTime, IEnumerable<BookResponse>> Books { get; set; }
    }
}
