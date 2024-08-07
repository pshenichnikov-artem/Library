using Library.Core.Domain.Entities;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IUserImageRepository
    {
        Task<UserImage?> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserImage>> GetAllAsync();
        Task<bool> AddAsync(UserImage image);
        Task<bool> UpdateAsync(UserImage image);
        Task<bool> DeleteAsync(Guid imageId);
    }
}
