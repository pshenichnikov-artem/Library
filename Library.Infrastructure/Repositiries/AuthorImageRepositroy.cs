using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class AuthorImageRepository : IAuthorImageRepository
    {
        private readonly ApplicationDbContext _db;

        public AuthorImageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<AuthorImage?> GetByAuthorIdAsync(Guid authorId)
        {
            return await _db.AuthorImages
                .Include(ui => ui.Author) // Include user to retrieve related user information
                .FirstOrDefaultAsync(ui => ui.AuthorID == authorId);
        }

        public async Task<IEnumerable<AuthorImage>> GetAllAsync()
        {
            return await _db.AuthorImages
                .Include(ui => ui.Author) // Include user to retrieve related user information
                .ToListAsync();
        }

        public async Task<bool> AddAsync(AuthorImage image)
        {
            _db.AuthorImages.Add(image);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AuthorImage image)
        {
            _db.AuthorImages.Update(image);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(AuthorImage image)
        {
            _db.AuthorImages.Remove(image);
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
