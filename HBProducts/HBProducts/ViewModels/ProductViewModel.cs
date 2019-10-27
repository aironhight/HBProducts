using HBProducts.Models;
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

        public ICommand URLClicked
        {
            get {
                return new Command<string>((url) => urlClicked(url));
            }
        }

        public ProductViewModel(Product product)
        {
            this.product = product;
        }

        public Product Product
        {
            set { SetProperty(ref product, value); }
            get { return product; }
        }

        public string Testing
        {
            get { return "Working!!!"; }
        }

        public void urlClicked(string url)
        {
            Debug.WriteLine("URL CLICKED CORRECT: " + url);
            Device.OpenUri(new Uri(url));
        }

    }
}
