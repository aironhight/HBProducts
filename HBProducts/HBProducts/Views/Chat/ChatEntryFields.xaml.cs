using HBProducts.Models;
using HBProducts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatEntryFields : ContentPage
    {
        private ChatManager manager;

        public ChatEntryFields()
        {
            InitializeComponent();
            entryLayout.WidthRequest = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density; //Set the width of the Entry manually...
            entryLayout.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density + 50;
            fullName.Text = Settings.GetUserData().Name;
            email.Text = Settings.GetUserData().Email;
            manager = new ChatManager();
        }

        private async void nextButtonclicked(object sender, EventArgs e)
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                AlertMessage("No internet", "You cannot open a session with no internet access...");
                return;
            }
            //Check if the fields are filled correctly
            if(fullName.Text == "")
            {
                AlertMessage("Error", "Name entry not filled.");
                return;
            }
            if( email.Text == "" || !email.Text.Contains("@"))
            {
                AlertMessage("Error", "E-mail entry not filled.");
                return;
            }
            setActivityIndicatorRunning(true);
            int sessionRes = await manager.GetSesionId(email.Text, fullName.Text);
            switch(sessionRes)
            {
                case -12:
                    AlertMessage("Error", "Internal server error. Please check your internet connection and try again!");
                    setActivityIndicatorRunning(false);
                    return;
                case -123:
                    AlertMessage("Error", "Exception in the API. Please check your internet connection and try again!");
                    setActivityIndicatorRunning(false);
                    return;
                case -420:
                    AlertMessage("Timeout Error", "The request has timed out. Please check your internet connection and try again!");
                    setActivityIndicatorRunning(false);
                    return;
                
            }
            await Navigation.PushAsync(new ChatPage(sessionRes));
            setActivityIndicatorRunning(false);
        }

        private void AlertMessage(string title, string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Alert(title, text);
            });
        }

        private async void Alert(string title, string text)
        {
            await DisplayAlert(title, text, "OK");
        }

        //Enable and disable fields + enable/disable Activity indicator
        private void setActivityIndicatorRunning(bool state)
        {
            indicator.IsRunning = state;
            indicator.IsVisible = state;
            fullName.IsEnabled = !state;
            email.IsEnabled = !state;
        }

    }
}