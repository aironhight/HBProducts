using HBProducts.Models;
using HBProducts.Services;
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
        private ProductList productList;
        private Boolean isLoading, noInternetConnection;
        private ProductManager manager;
        private INotifyView notifyView;

        public ICommand RefreshProducts
        {
            get
            {
                return new Command(() => requestProducts());
            }
        }

        public ProductsViewModel(INotifyView notifyView)
        {
            productList = new ProductList();
            manager = new ProductManager();
            this.notifyView = notifyView;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged; //Add an event handler for internet connectivity changes.
        }


        public async void requestProducts()
        {
            var networkAccess = Connectivity.NetworkAccess;

            if (networkAccess == NetworkAccess.Internet)
            {
                NoInternetConnection = false;
                IsLoading = true;
                string productListString = await manager.requestProductsAsync();
                IsLoading = false;

                if (productListString.Contains("Error:"))
                {
                    notifyView.notify("Error", productListString.Substring(6));
                    return;
                }

                ProductList = JsonConvert.DeserializeObject<ProductList>(productListString);
            } else {
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
            IsLoading = true;
            //ask the manager for the JSON string of the product
            string productString = await manager.getProductWithId(id);            

            //Check if the system threw an error
            if(productString.Contains("Error:"))
            {
                throw new SystemException(productString.Substring(6));
            }

            //Convert the string to a product and return it...
            Product toReturn = JsonConvert.DeserializeObject<Product>(productString);
            IsLoading = false;
            return toReturn;
        }

        //The method gets called on each internet connectivity change
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //If the connectivity changed to "Internet access" and there are no products loaded - make a request.
            if (e.NetworkAccess == Connectivity.NetworkAccess && ProductList.Products.Count == 0)
                requestProducts();
        }
    }
}
