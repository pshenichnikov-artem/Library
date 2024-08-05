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

            modelBuilder.Entity<BookAuthor>()
        .HasOne(ba => ba.Book)
        .WithMany(b => b.BookAuthors)
        .HasForeignKey(ba => ba.BookID)
        .OnDelete(DeleteBehavior.Cascade); // Prevent deletion of books if there are related book authors

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorID)
                .OnDelete(DeleteBehavior.Cascade); // Prevent deletion of authors if there are related book authors

            // Configure Book-BookImage one-to-many relationship
            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookImages)
                .WithOne(bi => bi.Book)
                .HasForeignKey(bi => bi.BookID)
                .OnDelete(DeleteBehavior.Cascade); // Delete images associated with book when book is deleted

            // Configure Author-AuthorImage one-to-many relationship
            modelBuilder.Entity<Author>()
                .HasMany(a => a.AuthorImages)
                .WithOne(ai => ai.Author)
                .HasForeignKey(ai => ai.AuthorID)
                .OnDelete(DeleteBehavior.Cascade); // Delete author images when author is deleted

            // Configure Book-BookFile one-to-many relationship
            modelBuilder.Entity<Book>()
                .HasMany(b => b.BookFiles)
                .WithOne(bf => bf.Book)
                .HasForeignKey(bf => bf.BookID)
                .OnDelete(DeleteBehavior.Cascade); // Delete files associated with book when book is deleted

            // Configure Book-Comment one-to-many relationship
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Comments)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of books if there are related comments

            // Configure ApplicationUser-Image one-to-many relationship
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.UserImages)
                .WithOne(ui => ui.User)
                .HasForeignKey(ui => ui.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Delete user images when user is deleted

            // Configure Rating-Book one-to-one relationship
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Book)
                .WithOne(b => b.Rating)
                .HasForeignKey<Rating>(r => r.BookID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of books if there are related ratings

            // Configure Comment-Book relationship
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Book)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BookID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of books if there are related comments

            // Configure BookImage-Book relationship
            modelBuilder.Entity<BookImage>()
                .HasOne(bi => bi.Book)
                .WithMany(b => b.BookImages)
                .HasForeignKey(bi => bi.BookID)
                .OnDelete(DeleteBehavior.Cascade); // Delete book images when book is deleted

            // Configure AuthorImage-Author relationship
            modelBuilder.Entity<AuthorImage>()
                .HasOne(ai => ai.Author)
                .WithMany(a => a.AuthorImages)
                .HasForeignKey(ai => ai.AuthorID)
                .OnDelete(DeleteBehavior.Cascade); // Delete author images when author is deleted

            // Configure UserImage-ApplicationUser relationship
            modelBuilder.Entity<UserImage>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserImages)
                .HasForeignKey(ui => ui.UserID)
                .OnDelete(DeleteBehavior.Cascade); // Delete user images when user is deleted

            modelBuilder.Entity<UserBookView>()
            .HasOne(ubv => ubv.User)
            .WithMany(u => u.UserBookViews)
            .HasForeignKey(ubv => ubv.UserID)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBookView>()
            .HasOne(ubv => ubv.Book)
            .WithMany(u => u.UserViews)
            .HasForeignKey(ubv => ubv.BookID)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
