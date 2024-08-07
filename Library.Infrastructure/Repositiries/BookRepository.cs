using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositiries
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Book?> GetByIdAsync(Guid bookId)
        {
            return await _db.Books
                .Include(b => b.BookImages)
                .Include(b => b.BookFiles)
                .Include(b => b.Comments)
                .ThenInclude(b => b.User)
                .Include(b => b.Rating)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(ba => ba.Owner)
                .FirstOrDefaultAsync(b => b.BookID == bookId);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _db.Books
                .Include(b => b.BookImages)
                .Include(b => b.Comments)
                .Include(b => b.Rating)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Book book)
        {
            _db.Books.Add(book);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            _db.Books.Update(book);
            return await SaveChangesAsync();
        }

        public async Task<Book> DeleteAsync(Book book)
        {
            _db.Books.Remove(book);
            await SaveChangesAsync();
            return book;
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
