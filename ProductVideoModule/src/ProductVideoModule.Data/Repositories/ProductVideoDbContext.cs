using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using ProductVideoModule.Data.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VideoLinkEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<VideoLinkEntity>().ToTable("VideoLinks");
            base.OnModelCreating(modelBuilder);
        }
    }
}
