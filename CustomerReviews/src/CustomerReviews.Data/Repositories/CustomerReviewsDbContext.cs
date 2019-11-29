using CustomerReviews.Data.Model;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace CustomerReviews.Data.Repositories
{
    public class CustomerReviewsDbContext : DbContextWithTriggers
    {
        public CustomerReviewsDbContext(DbContextOptions<CustomerReviewsDbContext> options)
           : base(options)
        {
        }

        protected CustomerReviewsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerReviewEntity>().ToTable("CustomerReview").HasKey(x => x.Id);
            modelBuilder.Entity<CustomerReviewEntity>().Property(x => x.Id).HasMaxLength(128);
            base.OnModelCreating(modelBuilder);
        }
    }
}
