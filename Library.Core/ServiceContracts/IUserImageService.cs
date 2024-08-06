using Library.Core.Domain.Entities;

namespace Library.Core.ServiceContracts
{
    public interface IUserImageService
    {
        Task<UserImage?> GetByIdAsync(Guid userImageId);
        Task<IEnumerable<UserImage>> GetByUserIdAsync(Guid userId);
        Task<UserImage> AddAsync(UserImage userImage);
        Task<UserImage> UpdateAsync(UserImage userImage);
        Task<bool> DeleteAsync(Guid userImageId);
    }
}
