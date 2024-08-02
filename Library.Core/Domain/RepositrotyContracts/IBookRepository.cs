using Library.Core.Domain.Entities;
using System.Linq.Expressions;
using System;
using System.Net;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IBookRepository
    {
        Task<Book> AddBookAsync(Book book);
        Task<List<Book>> GetAllBookAsync();
        Task<Book> GetBookByIDAsync(Guid bookID);
        Task<List<Book>> GetFilteredBookAsync(Expression<Func<Book, bool>> predicate);
        Task<bool> DeleteBookByIDAsync(Guid bookID);
        Task<Book> UpdateBookAsync(Book book);
    }
}
