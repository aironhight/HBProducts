using HBProducts.Models;
using HBProducts.Services;
using HBProducts.Views;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace HBProducts.ViewModels
{
    class ProductViewModel : BaseViewModel
    {
        private Product product;
        private INotifyView view;

        public ICommand URLClicked
        {
            get {
                return new Command<string>((url) => urlClicked(url));
            }
        }

        public ProductViewModel(Product product, INotifyView view)
        {
            this.product = product;
            this.view = view;
        }

        public Product Product
        {
            set { SetProperty(ref product, value); }
            get { return product; }
        }

        public async void urlClicked(string url)
        {
            Debug.WriteLine("URL CLICKED: " + url);
            
            switch(Device.RuntimePlatform)
            {
                case Device.iOS:
                    view.notify("openWebViewer", url);
                    break;

                default:
                    Device.OpenUri(new Uri(url));
                    break;
            }
        }

    }
}
