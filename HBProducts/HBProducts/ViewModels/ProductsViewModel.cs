using HBProducts.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HBProducts.ViewModels
{
    class ProductsViewModel : BaseViewModel
    {
        private readonly string productsURI = "https://hbproductswebapi.azurewebsites.net/api/Product";
        private ProductList productList;
        private HttpClient client;
        private Boolean isLoading;

        public ProductsViewModel()
        {
            productList = new ProductList();
            client = new HttpClient();
            requestProducts();
        }

        private async void requestProducts()
        {
            IsLoading = true;
            var response = await client.GetStringAsync(productsURI);
            IsLoading = false;
            string deserialized = JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String

            //Converts the JsonString to an object and updates the products in the productList
            //productList.Products = JsonConvert.DeserializeObject<ProductList>(deserialized).Products;
            ProductList = JsonConvert.DeserializeObject<ProductList>(deserialized);
        }

        public ProductList ProductList
        {
            set { SetProperty(ref productList, value); OnPropertyChanged("ProductList");
                  SetProperty(ref productList, value); OnPropertyChanged("ProductList");
                  SetProperty(ref productList, value); OnPropertyChanged("ProductList");
            }
            get { return productList; }
        }

        public Boolean IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); OnPropertyChanged("IsLoading"); }
        }

        public async Task<Product> GetProductWithId(int id)
        {
            var response = await client.GetStringAsync(productsURI + "/" + id);
            string deserialized = JsonConvert.DeserializeObject<string>(response);

            return JsonConvert.DeserializeObject<Product>(deserialized);
        }


    }
}
