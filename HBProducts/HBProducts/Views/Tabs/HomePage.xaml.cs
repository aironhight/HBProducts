using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HBProducts.Models;
using HBProducts.Views;
using HBProducts.ViewModels;
using Xamarin.Forms.Internals;

namespace HBProducts.Views
{
    [DesignTimeVisible(false)]
    public partial class HomePage : ContentPage
    {
   

        public HomePage()
        {
            InitializeComponent();
            //webView.Source = "https://www.hbproducts.dk/en/hb-products-link/news-archive.html";
            NavigationPage.SetHasNavigationBar(this, false);
        }

    }
}