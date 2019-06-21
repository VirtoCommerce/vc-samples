using System.IO;
using System.Reflection;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Core.Gateways;
using VirtoCommerce.WhatsAppNotification.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Data.Gateway;
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
            _container.RegisterType<IWhatsAppClient, TwilioWhatsApp>();

            var notificationManager = _container.Resolve<INotificationManager>();

            notificationManager.RegisterNotificationType(() => new OrderWhatsAppNotification(_container.Resolve<IWhatsAppNotificationSendingGateway>())
            {
                NotificationTemplate = new NotificationTemplate()
                {
                    Body = ReadFile($"{typeof(OrderWhatsAppNotification).Name}_body.htm")
                }
            });

            var eventHandlerRegistrar = _container.Resolve<IHandlerRegistrar>();

            eventHandlerRegistrar.RegisterHandler<OrderChangedEvent>(async (message, token) => await _container.Resolve<SendNotificationOrderChangedEventHandler>().Handle(message));
        }

        private static string ReadFile(string fileName)
        {
            var assembly = Assembly.GetAssembly(typeof(IWhatsAppClient));
            string result;

            using (var stream = assembly.GetManifestResourceStream($"VirtoCommerce.WhatsAppNotification.Data.Templates.{fileName}"))
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
