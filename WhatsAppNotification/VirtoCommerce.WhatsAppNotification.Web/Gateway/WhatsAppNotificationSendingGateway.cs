using System;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.WhatsAppNotification.Core.Gateway;
using VirtoCommerce.WhatsAppNotification.Data.WhatsAppClient;

namespace VirtoCommerce.WhatsAppNotification.Web.Gateway
{
    public class WhatsAppNotificationSendingGateway : IWhatsAppNotificationSendingGateway
    {
        private readonly IWhatsAppClient _client;

        public WhatsAppNotificationSendingGateway(IWhatsAppClient client)
        {
            _client = client;
        }

        public SendNotificationResult SendNotification(Notification notification)
        {
            var result = new SendNotificationResult();

            try
            {
                _client.SendMessage(notification.Recipient, notification.Body);

                result.IsSuccess = true;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.ErrorMessage = e.ToString();
            }

            return result;
        }

        public bool ValidateNotification(Notification notification)
        {
            return !string.IsNullOrEmpty(notification.Recipient) &&
                   !string.IsNullOrEmpty(notification.Body);
        }
    }
}
