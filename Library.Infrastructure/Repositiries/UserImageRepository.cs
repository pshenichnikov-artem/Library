using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class UserImageRepository : IUserImageRepository
    {
        private readonly ApplicationDbContext _db;

        public UserImageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<UserImage?> GetByUserIdAsync(Guid userId)
        {
            return await _db.UserImages
                .Include(ui => ui.User) // Include user to retrieve related user information
                .FirstOrDefaultAsync(ui => ui.UserID == userId);
        }

        public async Task<IEnumerable<UserImage>> GetAllAsync()
        {
            return await _db.UserImages
                .Include(ui => ui.User) // Include user to retrieve related user information
                .ToListAsync();
        }

        public async Task<bool> AddAsync(UserImage image)
        {
            _db.UserImages.Add(image);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UserImage image)
        {
            _db.UserImages.Update(image);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid imageId)
        {
            var image = await _db.UserImages
                .FirstOrDefaultAsync(ui => ui.UserImageID == imageId);

            if (image == null)
            {
                return false;
            }

            _db.UserImages.Remove(image);
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
