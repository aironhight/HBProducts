using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using HBProducts.Views.Tabs.Enquiry;
using SendGrid;
using System;

using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views.Tabs.Enquiry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnquiryPage : ContentPage, INotifyView
    {
        private EnquiryViewModel viewmodel;

        public EnquiryPage(Product product)
        {
            construct(product);
        }

        public EnquiryPage()
        {
            construct(null);
        }

        private void construct(Product product)
        {
            InitializeComponent();
            entryLayout.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density; //Set the width of the Entry manually...
            entryLayout.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density + 50;
            viewmodel = product != null ? new EnquiryViewModel(product, this) : new EnquiryViewModel(this);
            BindingContext = viewmodel;
            Title = "Personal Data";
        }

        public void notify(string type, params object[] list)
        {
            if (type.Equals("Error"))
            {
                //Display the error.
                DisplayAlert("Error", list[0].ToString(), "OK");
            }
        }

        private async void nextButtonclicked(object sender, EventArgs e)
        {

            if (fullName.Text.Length == 0)
            {
                await DisplayAlert("Warning", "Full name field empty.", "OK");
                return;
            }
            else if (company.Text.Length == 0)
            {
                await DisplayAlert("Warning", "Company field empty.", "OK");
                return;
            }
            else if (email.Text.Length == 0)
            {
                await DisplayAlert("Warning", "E-mail field empty.", "OK");
                return;
            }
            else if (telno.Text.Length == 0)
            {
                await DisplayAlert("Warning", "Telephone number field empty.", "OK");
                return;
            }

            if (checkbox.IsChecked)
                viewmodel.saveUserData(fullName.Text, company.Text, email.Text, telno.Text, country.Text);
            else
                viewmodel.clearUserData();

            await Navigation.PushAsync(new EnquiryMessagePage(fullName.Text, company.Text, email.Text, telno.Text, country.Text, viewmodel));
        }
    }
}