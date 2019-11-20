using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using HBProducts.Views.iOS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SendGrid;
using SendGrid.Helpers.Mail;
using HBProducts.Views.Tabs.Enquiry;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage, INotifyView
    {
        private ProductViewModel viewmodel;

        public ProductPage(Product product)
        {
            InitializeComponent();
            viewmodel = new ProductViewModel(product, this);
            BindingContext = viewmodel;
            Title = product.FullName;

            listViewNoURLData.HeightRequest = (240 * product.NoURLData.Count);
            listViewURLData.HeightRequest = (60 * product.URLData.Count);
        }

        private void threeDModelButtonClicked(object sender, EventArgs e)
        {
            open3dmodel();
        }

        private async void open3dmodel()
        {
            await Navigation.PushAsync(new ThreeDModelView(viewmodel.Product.ThreeDModel));
        }

        private void enquiryButtonClicked(object sender, EventArgs e)
        {
            openEnquiryPage();
        }

        public async void openEnquiryPage()
        {
            await Navigation.PushAsync(new EnquiryPage(viewmodel.Product));
        }

        public async void notify(string type, params object[] list)
        {
            switch (type)
            {
                case "openWebViewer":
                    openWebViewer(list[0].ToString());
                    break;
                case "favorited":
                    await DisplayAlert("Favourites", "The product has been added to favorites. The next time that you refresh the products page it will apear on top!", "OK");
                    break;
                case "unfavorited":
                    await DisplayAlert("Removed", "The next time that you refresh the products page the product will no longer appear on top!", "OK");
                    break;

            }
        }

        private async void openWebViewer(string url)
        {
            await Navigation.PushAsync(new WebViewerPage(url));
        }
    }
}