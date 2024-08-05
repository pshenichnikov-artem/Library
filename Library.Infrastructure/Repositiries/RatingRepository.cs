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

        public async Task<Rating?> GetByIdAsync(Guid ratingId)
        {
            return await _db.Ratings
                .Include(r => r.Book) // Include book to retrieve related book information
                .FirstOrDefaultAsync(r => r.RatingID == ratingId);
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            return await _db.Ratings
                .Include(r => r.Book) // Include book to retrieve related book information
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

        public async Task<bool> DeleteAsync(Guid ratingId)
        {
            var rating = await _db.Ratings
                .FirstOrDefaultAsync(r => r.RatingID == ratingId);

            if (rating == null)
            {
                return false;
            }

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
    }

}
