using System.Reflection;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Core.Gateways;
using VirtoCommerce.WhatsAppNotification.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Data.Gateways;
using VirtoCommerce.WhatsAppNotification.Data.Handlers;
using VirtoCommerce.WhatsAppNotification.Data.Senders;

namespace VirtoCommerce.WhatsAppNotification.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public override void Initialize()
        {
            _container.RegisterType<IWhatsAppNotificationSendingGateway, WhatsAppNotificationSendingGateway>();
            _container.RegisterType<IWhatsAppClient, TwilioWhatsAppClient>();
        }

        public override void PostInitialize()
        {
            var notificationManager = _container.Resolve<INotificationManager>();
            var assembly = Assembly.GetAssembly(typeof(IWhatsAppClient));

            notificationManager.RegisterNotificationType(() => new OrderWhatsAppNotification(_container.Resolve<IWhatsAppNotificationSendingGateway>())
            {
                NotificationTemplate = new NotificationTemplate()
                {
                    Body = assembly.GetManifestResourceStream($"VirtoCommerce.WhatsAppNotification.Data.Templates.{typeof(OrderWhatsAppNotification).Name}_body.htm").ReadToString()
                }
            });

            var eventHandlerRegistrar = _container.Resolve<IHandlerRegistrar>();

            eventHandlerRegistrar.RegisterHandler<OrderChangedEvent>(async (message, token) => await _container.Resolve<SendNotificationOrderChangedEventHandler>().Handle(message));
        }
    }
}
