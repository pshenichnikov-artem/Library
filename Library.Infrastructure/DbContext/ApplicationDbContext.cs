using Library.Core.Domain.Entities;
using Library.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace Library.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookFile> BookFiles { get; set; }
        public virtual DbSet<Image> Images { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<BookFile>().ToTable("BookFiles");
            modelBuilder.Entity<Image>().ToTable("Images");

            string bookJson = File.ReadAllText("C:/studies/Asp.Net_Core/Library/Library.Infrastructure/Book.json");
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(bookJson);
            foreach (var book in books)
            {
                modelBuilder.Entity<Book>().HasData(book);
            }

            string bookFileJson = File.ReadAllText("C:/studies/Asp.Net_Core/Library/Library.Infrastructure/BookFile.json");
            List<BookFile> bookFiles = JsonSerializer.Deserialize<List<BookFile>>(bookFileJson);
            foreach (var bookFile in bookFiles)
            {
                modelBuilder.Entity<BookFile>().HasData(bookFile);
            }

            string imageJson = File.ReadAllText("C:/studies/Asp.Net_Core/Library/Library.Infrastructure/Image.json");
            List<Image> images = JsonSerializer.Deserialize<List<Image>>(imageJson);
            foreach (var image in images)
            {
                modelBuilder.Entity<Image>().HasData(image);
            }
        }
    }
}
