using System;
namespace HBProducts
{
    public class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://bachproj.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=8ydPv6lUvOcBaozPOSnfyNUMQBrOqeCblKKoUoo7xmg=";
        public const string NotificationHubName = "NotificationTest";
        public static readonly string ProductsURI = "https://hbproductswebapi2.azurewebsites.net/api/Product";
        public static readonly string sendgridApiKey = "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";


        public static readonly string aboutCompany1 = "HB Products – dedicated to optimal solutions for level measurement and control of oil and refrigerants.";
        public static readonly string aboutCompany2 = "HB Products is a development-oriented company, which specialises in the development and production of sensors for industrial refrigeration systems. Apart from expertise within oil and refrigerant control, we have great know-how in the design and optimisation of industrial refrigeration systems. This knowledge enables us to develop and produce the best sensors!";
        public static readonly string aboutCompany3 = "Since its start more than 20 years ago, HB Products has attained a strong global position. This is the result of our ability to think in terms of new technological solutions, create trustworthy products, and provide a high level of service.";
        public static readonly string aboutParagraph1Label = "Products for centralised and decentralised control:";
        public static readonly string aboutParagraph1Text = "The product program consists of: sensors with switch function, sensors with analogue output signal (4-20 mA) and sensors with integrated direct control of magnetic/motor valves regulators for the control of pumps and/or valves";
        public static readonly string aboutParagraph2Label = "Service-friendly design – the sensible and sound financial choice for the user:";
        public static readonly string aboutParagraph2Text = "We always think of service in our product development. Our thorough and unique \"split design\", where the electronic part is easily removed from the mechanical part, allows for quick and easy testing or replacement of the electronics - without stoppages or loss of pressure. Our design of the mechanical parts, which require no maintenance at all, is just as intelligent.";
    }
}
