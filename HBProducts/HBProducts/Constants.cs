using System;
namespace HBProducts
{
    public class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://bachproj.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=8ydPv6lUvOcBaozPOSnfyNUMQBrOqeCblKKoUoo7xmg=";
        public const string NotificationHubName = "NotificationTest";
        public static readonly string ProductsURI = "https://hbproductswebapi.azurewebsites.net/api/Product";
        public static readonly string sendgridApiKey = "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";
    }
}
