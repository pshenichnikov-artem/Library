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
                .Include(u => u.UserImages) // Include related user images
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _db.Users
                .Include(u => u.UserImages) // Include related user images
                .ToListAsync();
        }

        public async Task<bool> AddAsync(ApplicationUser user)
        {
            _db.Users.Add(user);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(ApplicationUser user)
        {
            _db.Users.Update(user);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var user = await _db.Users
                .Include(u => u.UserImages)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            _db.Users.Remove(user);
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
