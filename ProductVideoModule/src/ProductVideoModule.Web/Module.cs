using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Services;
using ProductVideoModule.Data.Repositories;
using ProductVideoModule.Data.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace ProductVideoModule.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // database initialization
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("VirtoCommerce.ProductVideo") ?? configuration.GetConnectionString("VirtoCommerce");
            serviceCollection.AddDbContext<ProductVideoDbContext>(options => options.UseSqlServer(connectionString));

            //srevices
            serviceCollection.AddTransient<IVideoLinkRepository, VideoLinkRepsitoryBase>();
            serviceCollection.AddSingleton<Func<IVideoLinkRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IVideoLinkRepository>());
            serviceCollection.AddTransient<IProductVideoService, ProductVideoService>();
            serviceCollection.AddTransient<IProductVideoSearchService, ProductVideoSearchService>();


        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            // register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // register permissions
            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission() { GroupName = "Dummy", Name = x }).ToArray());

            // Ensure that any pending migrations are applied
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ProductVideoDbContext>())
                {
                    dbContext.Database.EnsureCreated();
                    dbContext.Database.Migrate();
                }
            }
        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
}
