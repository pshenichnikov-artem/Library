using Library.Core.Domain.Entities;
using Library.Core.DTO.Book;

namespace Library.Core.DTO.Account
{
    public class ApplicationUserResponse
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public IEnumerable<UserImage> UserImages { get; set; } = new List<UserImage>();
        public IEnumerable<UserBookView> RecentlyViewedBooks { get; set; } = new List<UserBookView>();
    }
}
