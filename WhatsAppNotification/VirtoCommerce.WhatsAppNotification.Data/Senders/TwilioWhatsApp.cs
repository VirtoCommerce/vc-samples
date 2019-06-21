using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.WhatsAppNotification.Data.Senders
{
    public class TwilioWhatsApp : IWhatsAppClient
    {
        private readonly ISettingsManager _settingsManager;

        public TwilioWhatsApp(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public void SendMessage(string recipient, string body)
        {
            var accountSid = _settingsManager.GetValue("VirtoCommerce.WhatsApp.Twilio.AccountSid", default(string));
            var authToken = _settingsManager.GetValue("VirtoCommerce.WhatsApp.Twilio.AuthToken", default(string));
            var accFrom = _settingsManager.GetValue("VirtoCommerce.WhatsApp.Twilio.FromAcc", default(string));

            if (!string.IsNullOrEmpty(accountSid) && !string.IsNullOrEmpty(accFrom) && !string.IsNullOrEmpty(authToken))
            {
                TwilioClient.Init(accountSid, authToken);

                var options =
                    new CreateMessageOptions(new PhoneNumber($"whatsapp:{recipient}"))
                    {
                        From = new PhoneNumber($"whatsapp:{accFrom}"),
                        Body = body
                    };

                var message = MessageResource.Create(options);
            }
        }
    }
}
