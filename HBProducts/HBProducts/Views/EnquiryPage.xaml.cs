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
            entryLayout.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density + 50;
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
            } else if(fullName.Text.Length == 0)
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

            String emailBody = "<p> <b> Full Name: </b>" + fullName.Text + "</p>"
                + "<p> <b> Company: </b>" + company.Text + "</p>"
                + "<p> <b> Country: </b>" + country.Text + "</p>"
                + "<p> <b> E-mail address: </b>" + email.Text + "</p>"
                + "<p> <b> Telephone nr.: </b>" + telno.Text + "</p>"
                + "<p> <b> Message: </b>" + entry.Text + "</p>";
            viewmodel.sendMail(emailBody);

            if (checkbox.IsChecked)
                viewmodel.saveUserData(fullName.Text, company.Text, email.Text, telno.Text, country.Text);
            else
                viewmodel.clearUserData();
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