using System.Data.Entity;
using CartModule2.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace CartModule2.Repositories
{
    public class Cart2Repository : CartRepositoryImpl
    {
        public Cart2Repository()
        {
        }

        public Cart2Repository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, interceptors)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart2Entity>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<Cart2Entity>().ToTable("Cart2");

            modelBuilder.Entity<LineItem2Entity>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<LineItem2Entity>().ToTable("CartLineItem2");

            base.OnModelCreating(modelBuilder);
        }
    }
}
