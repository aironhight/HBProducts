using HBProducts.Models;
using HBProducts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        private Product product;
        private ProductViewModel viewmodel;

        public ProductPage(Product product)
        {
            InitializeComponent();
            this.product = product;
            viewmodel = new ProductViewModel(product);
            BindingContext = viewmodel;
            Title = product.FullName;

            listViewNoURLData.HeightRequest = (240*product.NoURLData.Count);
            listViewURLData.HeightRequest = (60 * product.URLData.Count);
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
    }
}