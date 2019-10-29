using System;
using Plugin.AzurePushNotification;
using System;

namespace HBProducts
{
    public partial class Notification : Application
    {

        HBProducts.MainPage mPage;
        public Notification()
        {
            InitializeComponent();

            mPage = new AzurePushNotificationSample.MainPage()
            {
                Message = "Hello Azure Push Notifications!"
            };

            MainPage = new NavigationPage(mPage);
        }

        protected override async void OnStart()
        {

            // Handle when your app starts
            CrossAzurePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            };
            System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossAzurePushNotification.Current.Token}");

            CrossAzurePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    if (p.Data.ContainsKey("body"))
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            mPage.Message = $"{p.Data["body"]}";
                        });

                    }
                }
                catch (Exception ex)
                {

                }

            };

            CrossAzurePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //System.Diagnostics.Debug.WriteLine(p.Identifier);

                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Message = p.Identifier;
                    });
                }
                else if (p.Data.ContainsKey("color"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Navigation.PushAsync(new ContentPage()
                        {
                            BackgroundColor = Color.FromHex($"{p.Data["color"]}")

                        });
                    });

                }
                else if (p.Data.ContainsKey("aps.alert.title"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        mPage.Message = $"{p.Data["aps.alert.title"]}";
                    });

                }
            };
            CrossAzurePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Dismissed");
            };

            await CrossAzurePushNotification.Current.RegisterAsync(new string[] { "crossgeeks", "general" });
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
    }
}
