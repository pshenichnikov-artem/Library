using Library.Core.Domain.Entities;
using Library.Core.DTO;
using Microsoft.AspNetCore.Http;

namespace Library.Core.ServiceContracts
{
    public interface IUserImageService
    {
        Task<IEnumerable<UserImageResponse>> GetImagesByUserIdAsync(Guid? userId);
        Task<UserImageResponse?> AddImageAsync(IFormFile file, Guid? userId);
        Task<bool> UpdateImageAsync(Guid? imageId, IFormFile file);
        Task<bool> DeleteImageAsync(Guid? imageId);
        Task<bool> DeleteImagesByUserIdAsync(Guid? userId);
    }
}
