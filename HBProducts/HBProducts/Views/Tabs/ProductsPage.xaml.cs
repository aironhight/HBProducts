using HBProducts.Models;
using HBProducts.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        private ProductsViewModel viewmodel { get; set; }

        public ProductsPage()
        {
            InitializeComponent();

            viewmodel = new ProductsViewModel();
            //Binding ViewModel to View...
            BindingContext = viewmodel;
            productList.SelectedItem = null;
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            productList.SelectedItem = null;
            if (!viewmodel.NoInternetConnection)
            {
                try {
                    //There is internet connection.
                    Product dummyProduct = e.Item as Product; //This is the selected product, but it contains only product data for the thumbnail...
                    Product productClicked = await viewmodel.GetProductWithId(dummyProduct.Id); //So we request the product with all product data...
                    if(productClicked != null) { //Check if the request was successful
                        Debug.WriteLine("The selected product is: " + productClicked.Model);
                        await Navigation.PushAsync(new ProductPage(productClicked)); //launch the new page, parsing the selected product as parameter
                    }
                } catch (Exception ex)
                {
                    await DisplayAlert("System Error", ex.Message, "OK");
                }
            } else
            {
                //There is no internet connection
                await DisplayAlert("Alert", "There is no internet connection...", "OK");
            } 
        }
    }
}