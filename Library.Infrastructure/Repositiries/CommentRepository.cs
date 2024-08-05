using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Comment?> GetByIdAsync(Guid commentId)
        {
            return await _db.Comments
                .Include(c => c.Book) // Include book to retrieve related book information
                .FirstOrDefaultAsync(c => c.CommentID == commentId);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _db.Comments
                .Include(c => c.Book) // Include book to retrieve related book information
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Comment comment)
        {
            _db.Comments.Add(comment);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            _db.Comments.Update(comment);
            return await SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid commentId)
        {
            var comment = await _db.Comments
                .FirstOrDefaultAsync(c => c.CommentID == commentId);

            if (comment == null)
            {
                return false;
            }

            _db.Comments.Remove(comment);
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
