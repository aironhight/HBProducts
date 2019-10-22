using HBProducts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
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

        public static void urlClicked(string url)
        {
            Debug.WriteLine("URL CLICKED CORRECT: " + url);
            Device.OpenUri(new Uri(url));
        }
    }
}
