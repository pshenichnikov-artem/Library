using Library.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IAuthorImageRepository
    {
        Task<AuthorImage?> GetByAuthorIdAsync(Guid imageId);
        Task<IEnumerable<AuthorImage>> GetAllAsync();
        Task<bool> AddAsync(AuthorImage image);
        Task<bool> UpdateAsync(AuthorImage image);
        Task<bool> DeleteAsync(AuthorImage imageId);
    }
}
