using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class BookFileRepository : IBookFileRepository
    {
        private readonly ApplicationDbContext _db;

        public BookFileRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<BookFile?> GetByIdAsync(Guid fileId)
        {
            return await _db.BookFiles.FindAsync(fileId);
        }

        public async Task<IEnumerable<BookFile>> GetAllAsync()
        {
            return await _db.BookFiles.ToListAsync();
        }

        public async Task<bool> AddAsync(BookFile file)
        {
            _db.BookFiles.Add(file);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BookFile file)
        {
            _db.BookFiles.Update(file);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid fileId)
        {
            var file = await _db.BookFiles.FindAsync(fileId);
            if (file == null)
            {
                return false;
            }

            _db.BookFiles.Remove(file);
            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
