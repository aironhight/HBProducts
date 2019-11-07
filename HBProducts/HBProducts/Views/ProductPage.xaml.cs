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

        public void notify(string type, params object[] list)
        {
            switch (type)
            {
                case "openWebViewer":
                    openWebViewer(list[0].ToString());
                    break;
            }
        }

        private async void openWebViewer(string url)
        {
            await Navigation.PushAsync(new WebViewerPage(url));
        }
    }
}