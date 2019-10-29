using HBProducts.Models;
using Java.Net;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace HBProducts.ViewModels
{
    class ProductsViewModel : BaseViewModel
    {
        private readonly string productsURI = "https://hbproductswebapi.azurewebsites.net/api/Product";
        private ProductList productList;
        private HttpClient client;
        private Boolean isLoading, noInternetConnection;
        public ICommand RefreshProducts
        {
            get
            {
                return new Command(() => requestProducts());
            }
        }

        public ProductsViewModel()
        {
            client = new HttpClient();
            productList = new ProductList();
            requestProducts();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private async void requestProducts()
        {
            var networkAccess = Connectivity.NetworkAccess;

            if (networkAccess == NetworkAccess.Internet)
            {
                NoInternetConnection = false;

                IsLoading = true;
                try { 
                    var response = await client.GetStringAsync(productsURI);
                    IsLoading = false;
                    string deserialized = JsonConvert.DeserializeObject<string>(response); //Deserializes the response into a JSON String

                    //Converts the JsonString to an object and updates the products in the productList
                    //productList.Products = JsonConvert.DeserializeObject<ProductList>(deserialized).Products;
                    ProductList = JsonConvert.DeserializeObject<ProductList>(deserialized);
                } catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(1000);
                    requestProducts();
                }
            } else
            {
                NoInternetConnection = true;
            }

        }

        public ProductList ProductList
        {
            //Because Xamarin doesn't want to load the list otherwise... I don't like the code either :) 
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

        public Boolean NoInternetConnection
        {
            get { return noInternetConnection; }
            set { SetProperty(ref noInternetConnection, value); OnPropertyChanged("NoInternetConnection"); }
        }

        public async Task<Product> GetProductWithId(int id)
        {
            try { 
                var response = await client.GetStringAsync(productsURI + "/" + id);
                string deserialized = JsonConvert.DeserializeObject<string>(response);

                return JsonConvert.DeserializeObject<Product>(deserialized);
            } catch  (Java.Net.UnknownHostException netConnection)
            {
                return null;
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == Connectivity.NetworkAccess)
                requestProducts();
        }
    }
}
