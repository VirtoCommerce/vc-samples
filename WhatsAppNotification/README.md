# WhatsApp notification example
Sample sending Order change notifications to WhatsApp via Twilio or Woowa gateways

https://virtocommerce.com/docs/vc2devguide/working-with-platform-manager/basic-functions/working-with-notifications

## Twilio
https://www.twilio.com/docs/sms/whatsapp/quickstart/csharp

## WooWa
http://api.woo-wa.com/

To include WooWa gateway, replace Module.cs

```C#
_container.RegisterType<IWhatsAppClient, TwilioWhatsAppClient>();
```

by
```C#
_container.RegisterType<IWhatsAppClient, WooWaWhatsAppClient>();
```