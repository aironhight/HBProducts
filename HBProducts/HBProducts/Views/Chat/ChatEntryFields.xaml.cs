using HBProducts.Models;
using HBProducts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            fullName.Text = Settings.GetUserData().Name;
            email.Text = Settings.GetUserData().Email;
            manager = new ChatManager();
        }

        private async void nextButtonclicked(object sender, EventArgs e)
        {
            if(fullName.Text == "")
            {
                DisplayAlert("Error", "Name entry not filled.", "OK");
                return;
            }
            if( email.Text == "")
            {
                DisplayAlert("Error", "E-mail entry not filled.", "OK");
                return;
            }

            await Navigation.PushAsync(new ChatPage( await manager.GetSesionId(email.Text, fullName.Text)));
        }


    }
}