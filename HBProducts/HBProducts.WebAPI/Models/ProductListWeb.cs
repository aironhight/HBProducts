using System;
using System.Collections.ObjectModel;

namespace HBProducts.Models
{
    public class ProductListWeb
    {
        private ObservableCollection<ProductWeb> products = new ObservableCollection<ProductWeb>();

        public ProductListWen()
        {
            products = new ObservableCollection<ProductWeb>();
        }

        public ObservableCollection<Product> Products
        { 
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
