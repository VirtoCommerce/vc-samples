using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductVideoModule.Core;
using ProductVideoModule.Core.Events;
using ProductVideoModule.Core.Services;
using ProductVideoModule.Data.Handlers;
using ProductVideoModule.Data.Repositories;
using ProductVideoModule.Data.Services;
using ProductVideoModule.Web.Extensions;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace ProductVideoModule.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public IConfiguration Configuration { get; set; }
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // Fluent Validation
            serviceCollection.AddValidators();

            // Options
            serviceCollection.AddOptions<ProductVideoModuleOptions>().Bind(Configuration.GetSection("ExternalYoutubeApi")).ValidateDataAnnotations();

            // Database initialization
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("VirtoCommerce.ProductVideo") ?? configuration.GetConnectionString("VirtoCommerce");
            serviceCollection.AddDbContext<ProductVideoDbContext>(options => options.UseSqlServer(connectionString));

            // Srevices
            serviceCollection.AddTransient<IVideoLinkRepository, VideoLinkRepsitoryBase>();
            serviceCollection.AddSingleton<Func<IVideoLinkRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IVideoLinkRepository>());
            serviceCollection.AddTransient<IProductVideoService, ProductVideoService>();
            serviceCollection.AddTransient<IProductVideoSearchService, ProductVideoSearchService>();
            serviceCollection.AddTransient<IInternalYoutubeService, InternalYoutubeService>();

            // EventHandlers
            serviceCollection.AddTransient<ProductVideoAddedEventHandler>();
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

            var inProcessBus = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            inProcessBus.RegisterHandler<ProductVideoAddedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<ProductVideoAddedEventHandler>().Handle(message));
        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
}
