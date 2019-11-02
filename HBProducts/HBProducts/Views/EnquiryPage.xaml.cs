using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using SendGrid;
using System;

using System.Net;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnquiryPage : ContentPage, INotifyView
    {
        private EnquiryViewModel viewmodel;

        public EnquiryPage(Product product)
        {
            InitializeComponent();
            entryLayout.WidthRequest = DeviceDisplay.MainDisplayInfo.Width/DeviceDisplay.MainDisplayInfo.Density; //Set the width of the Entry manually...
            viewmodel = new EnquiryViewModel(product, this);
            BindingContext = viewmodel;
            Title = "Product enquiry";
        }

        private async void sendButtonClicked(object sender, EventArgs e)
        {
            if(entry.Text.Length == 0)
            {   //Alert the user if the entry field is empty.
                await DisplayAlert("Warning", "Entry field empty.", "OK");
                return;
            }
            viewmodel.sendMail(entry.Text);
        }

        public void notify(string type, params object[] list)
        {
            if (type.Equals("response"))
            {
                Response response = (Response)list[0];

                //Check if the enquiry was sent successfully
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    entry.Text = "";
                    DisplayAlert("Enquiry", "Enquiry sent successfully!", "OK");
                }
                else
                {
                    //Alert the user if not.
                    DisplayAlert("Enquiry", "Enquiry not successful. Error code:" + response.StatusCode.ToString(), "OK");
                }
            }
            else if (type.Equals("Error"))
            {
                //Display the error.
                DisplayAlert("Error", list[0].ToString(), "OK");
            }
        }
    }
}