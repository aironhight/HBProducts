﻿using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage, INotifyView
    {
        private ProductsViewModel viewmodel { get; set; }

        public ProductsPage()
        {
            InitializeComponent();

            viewmodel = new ProductsViewModel(this);
            //Binding ViewModel to View...
            BindingContext = viewmodel;
            productList.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(viewmodel.ProductList.ProductsCount() <= 0) //If the page is not filled with products - request products.
                Task.Factory.StartNew(() => viewmodel.requestProducts());
        }

        private async void OnItemSelected(object sender, ItemTappedEventArgs e)
        {
            productList.SelectedItem = null;
            if (!viewmodel.NoInternetConnection)
            {
                //There is internet connection.
                Task.Factory.StartNew(() => productClicked(e));
            }else
            {
                //There is no internet connection
                await DisplayAlert("Alert", "Turn on internet connectivity services.", "OK");
            } 
        }

        private async void productClicked(ItemTappedEventArgs e)
        {
            try
            {
                Product dummyProduct = e.Item as Product; //This is the selected product, but it contains only product data for the thumbnail...
                Product productClicked = await viewmodel.GetProductWithId(dummyProduct.Id); //So we request the product with all product data...
                Debug.WriteLine("The selected product is: " + productClicked.Model);
                Device.BeginInvokeOnMainThread(() => startProductpage(productClicked));
                 
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async void startProductpage(Product productClicked)
        {
            await Navigation.PushAsync(new ProductPage(productClicked)); //launch the new page, parsing the selected product as parameter
        }

        public async void notify(string type, params object[] list)
        {
            if(type.Equals("Error"))
            {
                await DisplayAlert("Error", list[0].ToString() + Environment.NewLine + "The page will automatically try to refresh.", "OK");
                viewmodel.requestProducts();
            }
        }
    }
}