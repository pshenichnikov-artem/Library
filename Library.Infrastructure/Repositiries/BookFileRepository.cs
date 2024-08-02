using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Repositiries
{
    public class BookFileRepository : IBookFileRepository
    {
        private readonly ApplicationDbContext _db;

        public BookFileRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<BookFile>> GetFileByBookID(Guid bookID)
        {
            return await _db.BookFiles
                .Where(f => f.BookID == bookID)
                .ToListAsync();
        }

        public async Task<BookFile?> GetFileByID(Guid id)
        {
            return await _db.BookFiles
                .FirstOrDefaultAsync(f => f.BookFileID == id);
        }

        public async Task<Image?> GetImageByID(Guid id)
        {
            return await _db.Images
                .FirstOrDefaultAsync(i => i.ImageID == id);
        }
    }
}
