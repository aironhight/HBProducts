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
            Product productClicked = await viewmodel.GetProductWithId(5);
            //Product productClicked = e.Item as Product;
            Debug.WriteLine("The selected product is: " + productClicked.Model);
            await Navigation.PushAsync(new ProductPage(productClicked));
        }
    }
}