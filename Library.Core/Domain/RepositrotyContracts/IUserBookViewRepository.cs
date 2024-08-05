using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IUserBookViewRepository
    {
        Task<UserBookView?> GetByIdAsync(Guid userBookViewId);
        Task<IEnumerable<UserBookView>> GetAllAsync();
        Task<bool> AddAsync(UserBookView userBookView);
        Task<bool> UpdateAsync(UserBookView userBookView);
        Task<bool> DeleteAsync(Guid userBookViewId);
        Task<bool> DeleteByUserIdAsync(Guid userId);
        Task<bool> DeleteByBookIdAsync(Guid bookId);
    }
}
