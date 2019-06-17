using System.IO;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Core.Gateway;
using VirtoCommerce.WhatsAppNotification.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Data.WhatsAppClient;
using VirtoCommerce.WhatsAppNotification.Web.Gateway;
using VirtoCommerce.WhatsAppNotification.Web.Handlers;

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

            var notificationTemplatePath = Path.Combine(ModuleInfo.FullPhysicalPath, "NotificationTemplates");

            notificationManager.RegisterNotificationType(() => new OrderWhatsAppNotification(_container.Resolve<IWhatsAppNotificationSendingGateway>())
            {
                NotificationTemplate = new NotificationTemplate()
                {
                    Body = ReadFile(notificationTemplatePath, $"{typeof(OrderWhatsAppNotification).Name}_body.htm")
                }
            });

            var eventHandlerRegistrar = _container.Resolve<IHandlerRegistrar>();

            eventHandlerRegistrar.RegisterHandler<OrderChangedEvent>(async (message, token) => await _container.Resolve<OrderChangedEventHandler>().Handle(message));
        }

        private string ReadFile(string directoryPath, string fileName)
        {
            string result = null;

            var filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
            {
                result = File.ReadAllText(filePath);
            }

            return result;
        }
    }
}
