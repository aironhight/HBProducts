using System;
namespace HBProducts
{
    public class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://bachproj.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=8ydPv6lUvOcBaozPOSnfyNUMQBrOqeCblKKoUoo7xmg=";
        public const string NotificationHubName = "NotificationTest";
        public static readonly string ProductsURI = "https://hbproductswebapi2.azurewebsites.net/api/Product";
        public static readonly string ChatURI = "https://hbproductschatapi.azurewebsites.net/api/Chat/";
        public static readonly string sendgridApiKey = "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";


        public static readonly string aboutCompany1 = "HB Products – dedicated to optimal solutions for level measurement and control of oil and refrigerants.";
        public static readonly string aboutCompany2 = "HB Products is a development-oriented company, which specialises in the development and production of sensors for industrial refrigeration systems. Apart from expertise within oil and refrigerant control, we have great know-how in the design and optimisation of industrial refrigeration systems. This knowledge enables us to develop and produce the best sensors!";
        public static readonly string aboutCompany3 = "Since its start more than 20 years ago, HB Products has attained a strong global position. This is the result of our ability to think in terms of new technological solutions, create trustworthy products, and provide a high level of service.";
        public static readonly string aboutParagraph1Label = "Products for centralised and decentralised control:";
        public static readonly string aboutParagraph1Text = "The product program consists of: sensors with switch function, sensors with analogue output signal (4-20 mA) and sensors with integrated direct control of magnetic/motor valves regulators for the control of pumps and/or valves";
        public static readonly string aboutParagraph2Label = "Service-friendly design – the sensible and sound financial choice for the user:";
        public static readonly string aboutParagraph2Text = "We always think of service in our product development. Our thorough and unique \"split design\", where the electronic part is easily removed from the mechanical part, allows for quick and easy testing or replacement of the electronics - without stoppages or loss of pressure. Our design of the mechanical parts, which require no maintenance at all, is just as intelligent.";


        public static readonly string q1 = "What are the LEDs indicating?(24VDC HBSR, HBSO1, HBSO2, HBSC2, HBOR)";
        public static readonly string a1 = "The LEDs have three different indications. When the sensor is blinking green the sensor is powered correctly and the sensor is not detecting level. When the LEDs are constant red the sensor is detecting level. Blinking red LEDs indicate an error e.g. the electronic part is not connected to the mechanical part correctly. For HBOR sensors red LEDs indicate OIL.";
        
        public static readonly string q2 = "What output configuration should I choose?(24VDC HBSR, HBSO1, HBSO2, HBSC2, HBOR)";
        public static readonly string a2 = "The sensor comes in a variety of different configurations. PNP configurations provide a 24VDC signal. NPN configurations provide a 0VDC signal. NO will provide the 24VDC or 0VDC signal respectively when the sensor detects level. NC will give the signal when the sensor is not detecting level. Available options are: PNP/NO, PNP/NC, NPN/NO and NPN/NC.";

        public static readonly string q3 = "How do I wire the sensor?(HBSR-SSR2, HBSO1-SSR2, HBSO2-SSR2, HBSC2-SSR2, HBOR-SSR2)";
        public static readonly string a3 = "Pin configuration is as follows:";

        public static readonly string q4 = "How do I choose between HBOS1 and HBSO2?";
        public static readonly string a4 = "HBSO1 is for PAO, POE & mineral oil. HBSO2 is for synthetic PAG oil.";

        public static readonly string q5 = "How do I wire the switch and what relay to choose?";
        public static readonly string a5 = "For PNP versions you connect the signal (pin 3) to the 24VDC+ terminal of the relay 24VDC- goes to ground. For NPN Versions you must connect signal (pin 3) to 24VDC- of the relay and 24VDC+ terminal to 24VDC. You can use any relay that meets the following specifications. Voltage: 24VDC. Min. coil resistance: 475Ω recommended relays are SCHRACK MT221024, OMRON G22A-432A.";



    }
}
