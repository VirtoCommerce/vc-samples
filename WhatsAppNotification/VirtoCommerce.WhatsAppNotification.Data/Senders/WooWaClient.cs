using System.Net;
using System.Text;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.WhatsAppNotification.Data.Senders
{
    public class WooWaClient : IWhatsAppClient
    {
        private readonly ISettingsManager _settingsManager;

        public WooWaClient(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public void SendMessage(string recipient, string message)
        {
            var license = _settingsManager.GetValue("VirtoCommerce.WhatsApp.WooWa.License", default(string));
            var domain = _settingsManager.GetValue("VirtoCommerce.WhatsApp.WooWa.Domain", default(string));
            var apiEndpoint = _settingsManager.GetValue("VirtoCommerce.WhatsApp.WooWa.APIEndpoint", default(string));

            if (!string.IsNullOrEmpty(license) && !string.IsNullOrEmpty(domain) && !string.IsNullOrEmpty(apiEndpoint))
            {
                var request = (HttpWebRequest)WebRequest.Create($"{apiEndpoint}/send-message");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                var data = $"license={license}&domain={domain}&wa_number={NormalizeRecipient(recipient)}&text={message}";
                var byteArray = Encoding.UTF8.GetBytes(data);

                request.ContentLength = byteArray.Length;

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                request.GetResponse();
            }
        }

        private static string NormalizeRecipient(string recepient)
        {
            if (recepient.StartsWith("+"))
            {
                recepient = recepient.Remove(0, 1);
            }

            return recepient;
        }
    }
}
