using Library.Core.DTO.Book.BookImage;
using Microsoft.AspNetCore.Http;

namespace Library.Core.ServiceContracts
{
    public interface IBookImageService
    {
        Task<BookImageResponse?> AddImageAsync(IFormFile file, Guid? bookId);
        Task<bool> UpdateImageAsync(Guid? imageId, IFormFile file);
        Task<bool> DeleteImageAsync(Guid? imageId);
        Task<IEnumerable<BookImageResponse>> GetImagesByBookIdAsync(Guid? bookId);
        Task<bool> DeleteImagesByBookIdAsync(Guid? bookId);
    }
}
