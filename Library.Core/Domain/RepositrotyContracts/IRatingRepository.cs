using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IRatingRepository
    {
        Task<List<Rating>> GetByUserIdAsync(Guid userID);
        Task<List<Rating>> GetByBookIdAsync(Guid bookID);
        Task<Rating?> GetByUserIdAndBookIdAsync(Guid userID, Guid bookID);
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<bool> AddAsync(Rating rating);
        Task<bool> UpdateAsync(Rating rating);
        Task<bool> DeleteAsync(Rating rating);
    }
}
