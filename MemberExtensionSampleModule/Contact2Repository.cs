using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ContactExtModule.Web.Model;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace ContactExtModule.Web
{
    public class Contact2Repository : CustomerRepositoryImpl
    {
        public Contact2Repository()
        {
        }

        public Contact2Repository(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, interceptors)
        {
            //Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Contact2
            modelBuilder.Entity<Contact2Entity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<Contact2Entity>().ToTable("Contact2");

            #endregion

            base.OnModelCreating(modelBuilder);
        }


        public IQueryable<Contact2Entity> Contact2s
        {
            get { return GetAsQueryable<Contact2Entity>(); }
        }
    }
}