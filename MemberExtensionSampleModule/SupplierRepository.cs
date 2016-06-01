using System.Data.Entity;
using System.Linq;
using MemberExtensionSampleModule.Web.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace MemberExtensionSampleModule.Web
{
    public class SupplierRepository : CustomerRepositoryImpl
    {
        public SupplierRepository()
        {
        }

        public SupplierRepository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, interceptors)
        {
            //Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Contact2
            modelBuilder.Entity<Contact2DataEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Contact2DataEntity>().ToTable("Contact2");

            #endregion

            #region Supplier
            modelBuilder.Entity<SupplierDataEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<SupplierDataEntity>().ToTable("Supplier");
            #endregion

            base.OnModelCreating(modelBuilder);
        }


        public IQueryable<Contact2DataEntity> Contact2s
        {
            get { return GetAsQueryable<Contact2DataEntity>(); }
        }

        public IQueryable<SupplierDataEntity> Suppliers
        {
            get { return GetAsQueryable<SupplierDataEntity>(); }
        }
    }
}