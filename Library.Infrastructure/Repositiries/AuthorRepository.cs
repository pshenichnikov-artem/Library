using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author?> GetByIdAsync(Guid authorId)
        {
            return await _context.Authors
                .Include(a => a.AuthorImages)
                .Include(a => a.BookAuthors)
                .ThenInclude(ba => ba.Book)
                .ThenInclude(ba => ba.BookImages)
                .FirstOrDefaultAsync(a => a.AuthorID == authorId);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .Include(a => a.AuthorImages)
                .Include(a => a.BookAuthors)
                .ThenInclude(ba => ba.Book)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Author author)
        {
            _context.Authors.Add(author);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
