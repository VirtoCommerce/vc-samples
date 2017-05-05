using System.Data.Entity;
using External.PriceModule.Web.Model;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.PricingModule.Data.Repositories;

namespace External.PriceModule.Web
{
    public class Price2Repository : PricingRepositoryImpl
    {
        public Price2Repository()
        {
        }

        public Price2Repository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, interceptors)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Price2Entity>().HasKey(x => x.Id).Property(x => x.Id);
            modelBuilder.Entity<Price2Entity>().ToTable("Price2");

            base.OnModelCreating(modelBuilder);
        }
    }
}