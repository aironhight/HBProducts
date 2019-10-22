using HBProducts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HBProducts.ViewModels
{
    class ProductViewModel : BaseViewModel
    {
        private Product product;

        public ProductViewModel(Product product)
        {
            this.product = product;
        }

        public Product Product
        {
            set { SetProperty(ref product, value); }
            get { return product; }
        }

        public ICommand ClickCommand => new Command<string>((url) =>
        {
            Device.OpenUri(new System.Uri(url));
        });

    }
}
