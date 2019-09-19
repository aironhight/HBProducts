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

namespace HBProducts.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class HomePage : ContentPage
    {
   

        public HomePage()
        {
            InitializeComponent();
            webView.Source = "https://www.hbproducts.dk/en/hb-products-link/news-archive.html";
        }
    }
}