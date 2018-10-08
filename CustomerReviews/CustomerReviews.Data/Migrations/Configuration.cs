namespace CustomerReviews.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CustomerReviews.Data.Model;

    public sealed class Configuration : DbMigrationsConfiguration<CustomerReviews.Data.Repositories.CustomerReviewRepository>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(CustomerReviews.Data.Repositories.CustomerReviewRepository context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.AddOrUpdate(new CustomerReviewEntity { Id = "1", ProductId = "0f7a77cc1b9a46a29f6a159e5cd49ad1", AuthorNickname = "Andrew Peters", Content = "Super!" });
            context.AddOrUpdate(new CustomerReviewEntity { Id = "2", ProductId = "0f7a77cc1b9a46a29f6a159e5cd49ad1", AuthorNickname = "Mr. Pumpkin", Content = "So so" });
            context.AddOrUpdate(new CustomerReviewEntity { Id = "3", ProductId = "0f7a77cc1b9a46a29f6a159e5cd49ad1", AuthorNickname = "John Doe", Content = "Liked that" });
        }
    }
}
