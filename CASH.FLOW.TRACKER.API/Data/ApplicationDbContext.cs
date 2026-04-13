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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);
                entity.Property(c => c.CategoryId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.UserId);

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

            //filters
            modelBuilder.Entity<Category>()
                .HasQueryFilter(c => !c.DeletedAt.HasValue);

            modelBuilder.Entity<Model.Transaction>()
                .HasQueryFilter(t => !t.DeletedAt.HasValue);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CreatedAt();
            UpdatedAt();
            DeletedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        public void CreatedAt()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            var userForNow = Guid.NewGuid();
            DateTime now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is Model.Transaction transaction)
                {
                    transaction.TransactionId = Guid.NewGuid();
                    //transaction.UserId = userForNow;
                    transaction.CreatedAt = now;
                } 
                else if (entry.Entity is Category category)
                {
                    category.CreatedAt = now;
                }
            }
        }


        public void UpdatedAt()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            DateTime now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is Model.Transaction transaction)
                {
                    transaction.UpdatedAt = now;
                }
                else if (entry.Entity is Category category)
                {
                    category.UpdatedAt = now;
                }
            }
        }

        public void DeletedAt()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Deleted);

            DateTime now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is Model.Transaction transaction)
                {
                    entry.State = EntityState.Modified;
                    transaction.DeletedAt = now;
                }
                else if (entry.Entity is Category category)
                {
                    entry.State = EntityState.Modified;
                    category.DeletedAt = now;
                }
            }
        }
    }
}
