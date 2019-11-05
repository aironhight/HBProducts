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

            CrossAzurePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PushAsync(new HomePage());
                });
            };

            

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //(Xamarin.Forms.Application.Current.MainPage as MainPage).IsGestureEnabled = false;
        }

        private void productsButtonClicked(object sender, System.EventArgs e)
        {
            // await Navigation.PushModalAsync(new NavigationPage(new ProductsPage()));
            string text = ((Button)sender).Text;
            var mdp = Xamarin.Forms.Application.Current.MainPage as MainPage;
            int id=1;
            switch (text)
            {
                case "Products":
                    id = 1;
                    break;
                case "Scan QR":
                    id = 2;
                    break;
                case "FAQ":
                    id = 3;
                    break;
                case "Contact us":
                    id = 4;
                    break;
                case "About":
                    id = 5;
                    break;

            }
            mdp.NavigateToPage(id);
        }
    }


}
