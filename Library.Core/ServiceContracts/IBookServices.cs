using Library.Core.Domain.Entities;
using Library.Core.DTO;
using Library.Core.DTO.Book;
using Library.Core.Enums;

namespace Library.Core.ServiceContracts
{
    public interface IBookService
    {
        Task<BookResponse?> GetByIdAsync(Guid? bookId);
        Task<IEnumerable<BookResponse>> GetAllAsync();
        Task<Book?> AddAsync(BookAddRequest? request,IEnumerable<Guid> authorID , Guid? ownerID);
        Task<bool> UpdateAsync(BookUpdateRequest? request);
        Task<BookResponse?> DeleteAsync(Guid? bookId);
        IEnumerable<BookResponse> GetSortedBooks(IEnumerable<BookResponse>? books, string? sortField, SortOrderOptions sortOrder);
        Task<IEnumerable<BookResponse>> GetFilteredBooksAsync(BookFilter? filter);
    }
}
