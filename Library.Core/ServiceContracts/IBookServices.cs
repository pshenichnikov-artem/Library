using Library.Core.DTO;
using Library.Core.Enums;

namespace Library.Core.ServiceContracts
{
    public interface IBookServices
    {
        Task<BookResponse> AddBook(BookAddRequest bookAddRequest, string ownerEmail , Guid? imageID = null);
        Task<List<BookResponse>> GetAllBooks();
        Task<BookResponse?> GetBookByBookID(Guid? bookID);
        Task<List<BookResponse>> GetFilteredBook(string? searchBy, string? searchString);
        Task<BookResponse?> DeleteBookByID(Guid? bookID);
       // Task<BookResponse> UpdateBook(BookAddRequest? bookRequest);//todo заменить bookAddRequest
        Task<List<BookResponse>> GetSortedBook(List<BookResponse> books, string sortBy, SortOrderOptions sortOrder);
    }
}
