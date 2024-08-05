using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class BookImageRepository : IBookImageRepository
    {
        private readonly ApplicationDbContext _db;

        public BookImageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<BookImage?> GetByIdAsync(Guid imageId)
        {
            return await _db.BookImages.FindAsync(imageId);
        }

        public async Task<IEnumerable<BookImage>> GetAllAsync()
        {
            return await _db.BookImages.ToListAsync();
        }

        public async Task<bool> AddAsync(BookImage image)
        {
            _db.BookImages.Add(image);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(BookImage image)
        {
            _db.BookImages.Update(image);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid imageId)
        {
            var image = await _db.BookImages.FindAsync(imageId);
            if (image == null)
            {
                return false;
            }

            _db.BookImages.Remove(image);
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
