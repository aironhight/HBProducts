using HBProducts.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace HBProducts.Models
{
    [Serializable]
    public class ProductList
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        public ProductList()
        {
            products = new ObservableCollection<Product>();
        }

        public ObservableCollection<Product> Products
        {
            set { products = value; }
            get { return products; }
        }

        //Methods to implement commands to manipulate the product list...
        public Product RemoveProduct(Product product)
        {
            Products.Remove(product);
            return product;
        }

        public void AddProduct(Product product)
        {
            Products.Add(product);
        }

        public int ProductsCount()
        {
            return products.Count;
        }
    }
}
