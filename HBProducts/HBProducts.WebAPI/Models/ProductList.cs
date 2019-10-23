using HBProducts.WebAPI.Models;
using System;
using System.Collections.ObjectModel;

namespace HBProducts.Models
{
    [Serializable]
    public class ProductList //: BaseViewModel
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        public ProductList()
        {
            products = new ObservableCollection<Product>();
        }

        public ObservableCollection<Product> Products
        {
            set { SetProperty(ref products, value); }
            get { return products; }
        }

        //Methods to implement commands to manipulate the product list...
        public void RemoveProduct(Product product)
        {
            Products.Remove(product);
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }
}
