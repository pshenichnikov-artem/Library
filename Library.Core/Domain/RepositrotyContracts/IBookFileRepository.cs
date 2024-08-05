using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IBookFileRepository
    {
        Task<BookFile?> GetByIdAsync(Guid fileId);
        Task<IEnumerable<BookFile>> GetAllAsync();
        Task<bool> AddAsync(BookFile file);
        Task<bool> UpdateAsync(BookFile file);
        Task<bool> DeleteAsync(Guid fileId);
    }
}
