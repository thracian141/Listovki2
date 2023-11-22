using Listovki.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Listovki.Data {
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => {
                entity.Property(e => e.Email).IsRequired();
                entity.HasKey(e => e.Email);
            });
        }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
    }
}
