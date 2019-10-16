﻿using HBProducts.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public ProductsPage()
        {
            InitializeComponent();

            //Binding ViewModel to View...
            BindingContext = new ProductsViewModel();
        }

        private void makeRequest(object sender, EventArgs e)
        {
            request();
        }

        private async void request()
        {
            HttpClient client = new HttpClient();

            var response = await client.GetStringAsync("http://teatapi.azurewebsites.net/api/Product");
            Console.WriteLine("RESPONSE: " + response);
            List<String> test = JsonConvert.DeserializeObject<List<String>>(response);
            foreach (String s in test)
            {
                Console.WriteLine("DESERIALIZED RESPONSE: " + s);
            }
        }
    }
}