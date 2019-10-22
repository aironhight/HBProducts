using HBProducts.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;

namespace HBProducts.ViewModels
{
    class ProductsViewModel : BaseViewModel
    {
        private readonly string productsURI = "http://teatapi.azurewebsites.net/api/Product";
        public event PropertyChangedEventHandler PropertyChanged;
        private ProductList productList;

        public ProductsViewModel()
        {
            productList = new ProductList();
            requestProducts();
        }

        private async void requestProducts()
        {
            /*HttpClient client = new HttpClient();

            var response = await client.GetStringAsync("http://teatapi.azurewebsites.net/api/Product");
            Console.WriteLine("RESPONSE: " + response);
            List<String> test = JsonConvert.DeserializeObject<List<String>>(response);*/

            /* Latest...
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(productsURI);
            productList = JsonConvert.DeserializeObject<ProductList>(response);
            Console.WriteLine("Server responded with: " + response); */


            productList.AddProduct(new Product("HBPS3", "Level Sensor", "https://www.hbproducts.dk/images/HBAC-2.png", new List<ProductData>()));
            productList.AddProduct(new Product("H3", "Oil Sensor", "https://www.hbproducts.dk/images/HBAC-2.png", new List<ProductData>()));
            productList.AddProduct(new Product("HAa3", "Refrigerant Sensor", "https://www.hbproducts.dk/images/HBAC-2.png", new List<ProductData>()));

        }

        public ProductList ProductList
        {
            set { SetProperty(ref productList, value); }
            get { return productList; }
        }
        
    }
}
