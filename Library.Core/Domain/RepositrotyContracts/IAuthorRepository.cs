using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(Guid authorId);
        Task<IEnumerable<Author>> GetAllAsync();
        Task<bool> AddAsync(Author author);
        Task<bool> UpdateAsync(Author author);
        Task<bool> DeleteAsync(Author author);
    }
}
