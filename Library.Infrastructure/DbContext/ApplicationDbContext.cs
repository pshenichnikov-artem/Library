using Library.Core.Domain.Entities;
using Library.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = default!;
        public DbSet<Author> Authors { get; set; } = default!;
        public DbSet<BookAuthor> BookAuthors { get; set; } = default!;
        public DbSet<BookImage> BookImages { get; set; } = default!;
        public DbSet<AuthorImage> AuthorImages { get; set; } = default!;
        public DbSet<BookFile> BookFiles { get; set; } = default!;
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<Comment> Comments { get; set; } = default!;
        public DbSet<Rating> Ratings { get; set; } = default!;
        public DbSet<UserBookView> UserBookViews { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Book entity
            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookAuthors)
                .WithOne(ba => ba.Book)
                .HasForeignKey(ba => ba.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookImages)
                .WithOne(bi => bi.Book)
                .HasForeignKey(bi => bi.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookFiles)
                .WithOne(bf => bf.Book)
                .HasForeignKey(bf => bf.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Author entity
            modelBuilder.Entity<Author>()
                .HasMany(a => a.BookAuthors)
                .WithOne(ba => ba.Author)
                .HasForeignKey(ba => ba.AuthorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Author>()
                .HasMany(a => a.AuthorImages)
                .WithOne(ai => ai.Author)
                .HasForeignKey(ai => ai.AuthorID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Rating entity
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Book)
                .WithMany(b => b.Rating)
                .HasForeignKey(r => r.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Comment entity
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.Cascade); 

            // Configure BookImage entity
            modelBuilder.Entity<BookImage>()
                .HasOne(bi => bi.Book)
                .WithMany(b => b.BookImages)
                .HasForeignKey(bi => bi.BookID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure AuthorImage entity
            modelBuilder.Entity<AuthorImage>()
                .HasOne(ai => ai.Author)
                .WithMany(a => a.AuthorImages)
                .HasForeignKey(ai => ai.AuthorID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ApplicationUser entity
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.UserImages)
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Comments)
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure UserBookView entity
            modelBuilder.Entity<UserBookView>()
                .HasOne(ubv => ubv.User)
                .WithMany(u => u.UserBookViews)
                .HasForeignKey(ubv => ubv.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBookView>()
                .HasOne(ubv => ubv.Book)
                .WithMany(b => b.UserViews)
                .HasForeignKey(ubv => ubv.BookID)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
