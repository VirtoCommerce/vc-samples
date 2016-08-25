using System.Data.Entity;
using System.Linq;
using MemberExtensionSampleModule.Web.Model;
using VirtoCommerce.CustomerModule.Data.Model;
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

            #region SupplierReviews
            modelBuilder.Entity<SupplierReviewDataEntity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<SupplierReviewDataEntity>().ToTable("SupplierReview");

            modelBuilder.Entity<SupplierReviewDataEntity>().HasRequired(m => m.Supplier)
                                                 .WithMany(m => m.Reviews).HasForeignKey(m => m.SupplierId)
                                                 .WillCascadeOnDelete(true);
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

        public IQueryable<SupplierReviewDataEntity> SupplierReviews
        {
            get { return GetAsQueryable<SupplierReviewDataEntity>(); }
        }

        public override MemberDataEntity[] GetMembersByIds(string[] ids, string responseGroup = null, string[] memberTypes = null)
        {
            var retVal = base.GetMembersByIds(ids, responseGroup, memberTypes);
            var reviews = SupplierReviews.Where(x => ids.Contains(x.SupplierId)).ToArray();
            return retVal;
        }
    }
}