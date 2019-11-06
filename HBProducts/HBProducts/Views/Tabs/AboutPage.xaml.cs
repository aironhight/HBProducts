using HBProducts.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel viewmodel;
        public AboutPage()
        {
            InitializeComponent();
            viewmodel = new AboutViewModel();
            BindingContext = viewmodel; 
        }
    }
}