using Library.Core.DTO.Author.AuthorImage;
using Microsoft.AspNetCore.Http;

namespace Library.Core.ServiceContracts
{
    public interface IAuthorImageService
    {
        Task<AuthorImageResponse?> AddImageAsync(IFormFile file, Guid? authorId);
        Task<bool> UpdateImageAsync(Guid? imageId, IFormFile file);
        Task<IEnumerable<AuthorImageResponse>> GetImagesByAuthorIdAsync(Guid? authorId);
        Task<bool> DeleteImageAsync(Guid? imageId);
        Task<bool> DeleteImagesByUserIdAsync(Guid? authorId);
    }
}
