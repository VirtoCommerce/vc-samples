using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Data;

namespace ProductVideoModule.Data.Repositories
{
    public class ProductVideoDbContext : DbContextWithTriggers
    {
        public ProductVideoDbContext(DbContextOptions<ProductVideoDbContext> options)
            : base(options)
        {
        }

        protected ProductVideoDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
