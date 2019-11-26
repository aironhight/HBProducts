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
            //Check if the fields are filled correctly
            if(fullName.Text == "")
            {
                DisplayAlert("Error", "Name entry not filled.", "OK");
                return;
            }
            if( email.Text == "" || !email.Text.Contains("@"))
            {
                DisplayAlert("Error", "E-mail entry not filled.", "OK");
                return;
            }
            setActivityIndicatorRunning(true);
            await Navigation.PushAsync(new ChatPage( await manager.GetSesionId(email.Text, fullName.Text)));
            setActivityIndicatorRunning(false);
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