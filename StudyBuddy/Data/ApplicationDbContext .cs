using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyBuddy.Models;

namespace StudyBuddy.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Blog>()
                .HasMany(b => b.Tags)
                .WithMany(t => t.Blogs);


            builder.Entity<Note>()
                .HasOne(n => n.Creator)
                .WithMany(u => u.PersonalNotes)
                .HasForeignKey(n => n.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<ApplicationUser>()
                .HasMany(u => u.SavedNotes)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserSavedNotes"));


            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFollowers"));
        }
    }
}
