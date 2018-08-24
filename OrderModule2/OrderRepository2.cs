using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using OrderModule2.Model;
using OrderModule2.Web.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace OrderModule2
{
    public class OrderRepository2 : OrderRepositoryImpl
    {
        public OrderRepository2()
        {
        }

        public OrderRepository2(string nameOrConnectionString, params IInterceptor[] interceptors)
            : base(nameOrConnectionString, interceptors)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region CustomerOrder2
            modelBuilder.Entity<CustomerOrder2Entity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<CustomerOrder2Entity>().ToTable("CustomerOrder2");
            #endregion
          
            #region LineItem2
            modelBuilder.Entity<LineItem2Entity>().HasKey(x => x.Id)
                .Property(x => x.Id);
            modelBuilder.Entity<LineItem2Entity>().ToTable("OrderLineItem2");
            #endregion

            #region OrderInvoice
            modelBuilder.Entity<InvoiceEntity>().HasKey(x => x.Id)
            .Property(x => x.Id);
            modelBuilder.Entity<InvoiceEntity>().ToTable("OrderInvoice");

            modelBuilder.Entity<InvoiceEntity>().HasRequired(m => m.CustomerOrder2)
                                                 .WithMany(m => m.Invoices).HasForeignKey(m => m.CustomerOrder2Id)
                                                 .WillCascadeOnDelete(true);
            #endregion

            base.OnModelCreating(modelBuilder);
        }


        public IQueryable<CustomerOrder2Entity> CustomerOrders2
        {
            get { return GetAsQueryable<CustomerOrder2Entity>(); }
        }

        public IQueryable<InvoiceEntity> Invoices
        {
            get { return GetAsQueryable<InvoiceEntity>(); }
        }

        public override CustomerOrderEntity[] GetCustomerOrdersByIds(string[] ids, CustomerOrderResponseGroup responseGroup)
        {
            var retVal = base.GetCustomerOrdersByIds(ids, responseGroup);
            var invoices = Invoices.Where(x => ids.Contains(x.CustomerOrder2Id)).ToArray();
            return retVal;
        }

    }
}