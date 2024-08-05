using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid bookId);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<bool> AddAsync(Book book);
        Task<bool> UpdateAsync(Book book);
        Task<Book> DeleteAsync(Book book);
    }
}
