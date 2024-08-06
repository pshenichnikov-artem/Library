using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class UserBookViewRepository : IUserBookViewRepository
    {
        private readonly ApplicationDbContext _db;

        public UserBookViewRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserBookView?> GetByUserIdAsync(Guid userId)
        {
            return await _db.UserBookViews
                .Include(u => u.User)
                .ThenInclude(u => u.UserImages)
                .Include(u => u.Book)
                .ThenInclude(u => u.BookAuthors)
                .ThenInclude(u => u.Author)
                .Include(U=>U.Book.BookImages)
                .FirstOrDefaultAsync(u => u.UserID == userId);
        }

        public async Task<IEnumerable<UserBookView>> GetAllAsync()
        {
            return await _db.UserBookViews
                .Include(u => u.User)
                .ThenInclude(u => u.UserImages)
                .Include(u => u.Book)
                .ThenInclude(u => u.BookAuthors)
                .ThenInclude(u => u.Author)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(UserBookView userBookView)
        {
            await _db.UserBookViews.AddAsync(userBookView);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(UserBookView userBookView)
        {
            _db.UserBookViews.Update(userBookView);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid userBookViewId)
        {
            var entity = await _db.UserBookViews.FindAsync(userBookViewId);
            if (entity == null)
            {
                return false;
            }

            _db.UserBookViews.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByUserIdAsync(Guid userId)
        {
            var entities = _db.UserBookViews.Where(ubv => ubv.UserID == userId);
            _db.UserBookViews.RemoveRange(entities);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByBookIdAsync(Guid bookId)
        {
            var entities = _db.UserBookViews.Where(ubv => ubv.BookID == bookId);
            _db.UserBookViews.RemoveRange(entities);
            return await _db.SaveChangesAsync() > 0;
        }
    }

}
