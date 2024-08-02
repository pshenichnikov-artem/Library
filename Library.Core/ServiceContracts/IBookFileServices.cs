using Library.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.ServiceContracts
{
    public interface IBookFileServices
    {
        Task<BookFile?> GetFileById(Guid? fileID);
        Task<List<BookFile>?> GetFileByBookID(Guid? bookID);
        Task<Image?> GetImageByBookID(Guid? imageID);
        Task<BookFile?> AddBookFile(Guid? bookID, IFormFile file);
        Task<Image> AddImageFile(IFormFile file);
    }
}
