using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(Guid commentId);
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<bool> AddAsync(Comment comment);
        Task<bool> UpdateAsync(Comment comment);
        Task<bool> DeleteAsync(Guid commentId);
    }
}
