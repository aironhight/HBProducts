using System.ComponentModel;
using Xamarin.Forms;
using Urho;
using Urho.Forms;

namespace HBProducts.Views
{
    [DesignTimeVisible(false)]
    public partial class HomePage : ContentPage
    {

        private ThreeDModelViewer urhoApp;

        public HomePage()
        {
            InitializeComponent();
            //webView.Source = "https://www.hbproducts.dk/en/hb-products-link/news-archive.html";
            NavigationPage.SetHasNavigationBar(this, false);
        }

       protected override async void OnAppearing()
        {
            base.OnAppearing();

            StartUrho();
            //await TDMUrhoSurface.Show<ThreeDModelViewer>(new Urho.ApplicationOptions(assetsFolder: null));
        }

        private async void StartUrho()
        {
            urhoApp = await TDMUrhoSurface.Show<ThreeDModelViewer>(new Urho.ApplicationOptions("Data") { Orientation = Urho.ApplicationOptions.OrientationType.LandscapeAndPortrait });
        }

        private void StartScene(object sender, System.EventArgs e)
        {
            StartUrho();
        }
    }
}