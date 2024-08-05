using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IRatingRepository
    {
        Task<Rating?> GetByIdAsync(Guid ratingId);
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<bool> AddAsync(Rating rating);
        Task<bool> UpdateAsync(Rating rating);
        Task<bool> DeleteAsync(Guid ratingId);
    }
}
