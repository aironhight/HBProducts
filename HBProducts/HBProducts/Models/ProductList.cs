using HBProducts.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HBProducts.Models
{
    class ProductList : BaseViewModel
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private string testString = "Testing a string for binding";

        public ProductList()
        {
            products = new ObservableCollection<Product>();
        }

        public ObservableCollection<Product> Products
        {
            set { SetProperty(ref products, value); }
            get { return products; }
        }

        public String TestString
        {
            set { SetProperty(ref testString, value); }
            get { return testString; }
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
