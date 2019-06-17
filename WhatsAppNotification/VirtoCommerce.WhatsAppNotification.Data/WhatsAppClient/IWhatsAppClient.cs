namespace VirtoCommerce.WhatsAppNotification.Data.WhatsAppClient
{
    public interface IWhatsAppClient
    {
        void SendMessage(string recipient, string body);
    }
}
