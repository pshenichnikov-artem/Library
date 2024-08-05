using Library.Core.DTO.Book.BookFile;
using Microsoft.AspNetCore.Http;

namespace Library.Core.ServiceContracts
{
    public interface IBookFileService
    {
        Task<BookFileResponse?> AddAsync(IFormFile file, Guid? bookId);
        Task<bool> UpdateAsync(Guid? fileId, IFormFile file);
        Task<bool> DeleteAsync(Guid? fileId);
        Task<IEnumerable<BookFileResponse>> GetFilesByBookIdAsync(Guid? bookId);
        Task<bool> DeleteFilesByBookIdAsync(Guid? bookId);
        Task<BookFileResponse?> GetFilesByIDAsync(Guid? bookFileID);
        Task<byte[]> GetFileBytesAsync(string fileName);
    }
}
