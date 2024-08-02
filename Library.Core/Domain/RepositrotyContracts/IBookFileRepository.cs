
using Library.Core.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IBookFileRepository
    {
        Task<BookFile?> GetFileByID(Guid id);
        Task<List<BookFile>> GetFileByBookID(Guid guid);
        Task<Image> GetImageByID(Guid id);
    }
}
