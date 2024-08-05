using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IBookImageRepository
    {
        Task<BookImage?> GetByIdAsync(Guid imageId);
        Task<IEnumerable<BookImage>> GetAllAsync();
        Task<bool> AddAsync(BookImage image);
        Task<bool> UpdateAsync(BookImage image);
        Task<bool> DeleteAsync(Guid imageId);
    }
}
