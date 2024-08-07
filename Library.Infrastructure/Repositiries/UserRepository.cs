using Library.Core.Domain.IdentityEntities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ApplicationUser?> GetByIdAsync(Guid userId)
        {
            return await _db.Users
                .Include(u => u.UserImages)
                .Include(u => u.UserBookViews)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _db.Users
                .Include(u => u.UserImages)
                .Include(u => u.UserBookViews)
                .ToListAsync();
        }

        public async Task<bool> Update(ApplicationUser user)
        {
            var userInDb = await _db.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if(userInDb == null)
                return false;

            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            
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
