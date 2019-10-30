using System.ComponentModel;
using Xamarin.Forms;
using Urho;
using Urho.Forms;
using Plugin.AzurePushNotification;

namespace HBProducts.Views
{
    public partial class HomePage : ContentPage

    {

        public HomePage()
        {
            InitializeComponent();

            //webView.Source = "https://www.hbproducts.dk/en/hb-products-link/news-archive.html";
            CrossAzurePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PushAsync(new HomePage());

                    });

                
            };

        }


    }


}
