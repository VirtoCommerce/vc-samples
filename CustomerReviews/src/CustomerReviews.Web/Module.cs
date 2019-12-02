using System;
using System.Linq;
using CustomerReviews.Core;
using CustomerReviews.Core.Services;
using CustomerReviews.Data.Repositories;
using CustomerReviews.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Extensions;

namespace CustomerReviews.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            serviceCollection.AddTransient<ICustomerReviewRepository, CustomerReviewRepository>();
            var connectionString = configuration.GetConnectionString("VirtoCommerce.CustomerReviews") ?? configuration.GetConnectionString("VirtoCommerce");
            serviceCollection.AddDbContext<CustomerReviewsDbContext>(options => options.UseSqlServer(connectionString));
            serviceCollection.AddSingleton<Func<ICustomerReviewRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<ICustomerReviewRepository>());

            serviceCollection.AddTransient<ICustomerReviewSearchService, CustomerReviewSearchService>();
            serviceCollection.AddTransient<ICustomerReviewService, CustomerReviewService>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);
            //Register settings for type Store
            settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.AllSettings, "Store");

            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission()
                {
                    GroupName = "CustomerReview",
                    ModuleId = ModuleInfo.Id,
                    Name = x
                }).ToArray());


            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerReviewsDbContext>();
                dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }
        }

        public void Uninstall()
        {
        }
    }
}
