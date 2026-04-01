using BUGET.TRACKER.API.Model;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace BUGET.TRACKER.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Model.Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Model.Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Model.Transaction>()
                .HasOne(t => t.Category)
                .WithMany(t => t.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .IsRequired();

            modelBuilder.Entity<Model.Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<Model.Transaction>()
                .HasIndex(t => t.CategoryId);
        }

    }
}
