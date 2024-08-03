using Library.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Library.Core.ServiceContracts
{
    public interface IBookFileServices
    {
        Task<BookFile?> GetFileById(Guid? fileID);
        Task<List<BookFile>?> GetFileByBookID(Guid? bookID);     
        Task<BookFile?> AddBookFile(Guid? bookID, IFormFile file);
        Task<bool> DeleteBookFileByBookID(Guid? bookID);

        //Image
        Task<Image> AddImageFile(IFormFile file);
        Task<Image?> GetImageByID(Guid? imageID);
    }
}
