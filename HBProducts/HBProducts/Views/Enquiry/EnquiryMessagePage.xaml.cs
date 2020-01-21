using HBProducts.Services;
using HBProducts.ViewModels;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views.Tabs.Enquiry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnquiryMessagePage : ContentPage, INotifyView
    {
        private string fullName, company, email, telNo, country;
        private EnquiryViewModel viewModel;

        //fullName.Text, company.Text, email.Text, telno.Text, country.Text
        public EnquiryMessagePage(string fullName, string company, string email, string telNo, string country, EnquiryViewModel viewModel)
        {
            InitializeComponent();
            entryLayout.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density; //Set the width of the Entry manually...
            entryLayout.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density + 50;
            this.fullName = fullName;
            this.company = company;
            this.email = email;
            this.telNo = telNo;
            this.country = country;
            this.viewModel = viewModel;
            viewModel.INotifyView = this;
            BindingContext = viewModel;
            Title = "Enquiry";
        }

        public async void notify(string type, params object[] list)
        {
            if (type.Equals("response"))
            {
                Response response = (Response)list[0];

                //Check if the enquiry was sent successfully
                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    editor.Text = "";
                    await DisplayAlert("Enquiry", "Enquiry sent successfully!", "OK");
                    Navigation.PopAsync();
                    Navigation.PopAsync();

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

        private async void sendButtonClicked(object sender, EventArgs e)
        {
            String emailBody = "<p> <b> Full Name: </b>" + fullName + "</p>"
                + "<p> <b> Company: </b>" + company + "</p>"
                + "<p> <b> Country: </b>" + country + "</p>"
                + "<p> <b> E-mail address: </b>" + email + "</p>"
                + "<p> <b> Telephone nr.: </b>" + telNo + "</p>"
                + "<p> <b> Message: </b>" + editor.Text + "</p>";
                        viewModel.sendMail(emailBody);
        }

    }
}