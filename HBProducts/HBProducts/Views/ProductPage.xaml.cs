using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using HBProducts.Views.iOS;
using System;
using System.Collections.Generic;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage, INotifyView
    {
        private Product product;
        private ProductViewModel viewmodel;

        public ProductPage(Product product)
        {
            InitializeComponent();
            this.product = product;
            viewmodel = new ProductViewModel(product, this);
            BindingContext = viewmodel;
            Title = product.FullName;

            listViewNoURLData.HeightRequest = (240*product.NoURLData.Count);
            listViewURLData.HeightRequest = (60 * product.URLData.Count);
        }

        public ProductPage(Product product, Element parentPage)
        {
            InitializeComponent();
            this.product = product;
            viewmodel = new ProductViewModel(product, this);
            BindingContext = viewmodel;
            Title = product.FullName;

            listViewNoURLData.HeightRequest = (240 * product.NoURLData.Count);
            listViewURLData.HeightRequest = (60 * product.URLData.Count);

            this.Parent = parentPage;
        }

        private void threeDModelButtonClicked(object sender, EventArgs e)
        {
            open3dmodel();
        }

        private async void open3dmodel()
        {
            await Navigation.PushAsync(new ThreeDModelView(product.ThreeDModel));
        }

        private void enquiryButtonClicked(object sender, EventArgs e)
        {
            openEnquiryPage();
        }

        private async void openEnquiryPage()
        {
            try
            {
                List<string> empty = new List<string>();
                empty.Add("info@hbproducts.dk");
                var message = new EmailMessage
                {
                    Subject = "Enquiry about " + product.FullName,
                    Body = "Write product enquiry...",
                    To = empty,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
                }
                catch (FeatureNotSupportedException fbsEx)
                {
                    // Email is not supported on this device
                }
                catch (Exception ex)
                {
                    // Some other exception occurred
                }
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