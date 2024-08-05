using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _db;

        public RatingRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            return await _db.Ratings
                .Include(r => r.User) // Include book to retrieve related book information
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Rating rating)
        {
            _db.Ratings.Add(rating);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Rating rating)
        {
            _db.Ratings.Update(rating);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Rating rating)
        {
            _db.Ratings.Remove(rating);
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

        public async Task<List<Rating>> GetByUserIdAsync(Guid userID)
        {
            return await _db.Ratings
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.UserID == userID)
                .ToListAsync();
        }

        public async Task<List<Rating>> GetByBookIdAsync(Guid bookID)
        {
            return await _db.Ratings
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.BookID == bookID)
                .ToListAsync();
        }

        public async Task<Rating?> GetByUserIdAndBookIdAsync(Guid userID, Guid bookID)
        {
            return await _db.Ratings
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.UserID == userID && r.BookID == bookID);
        }
    }

}
