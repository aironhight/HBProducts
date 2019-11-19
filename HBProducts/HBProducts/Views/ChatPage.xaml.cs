using HBProducts.Models;
using HBProducts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        private ChatManager manager;
        

        public ChatPage()
        {
            InitializeComponent();
            manager = new ChatManager();
        }

        private void SendRequestClicked(object sender, EventArgs e)
        {
            Message message = new Message(false, "Test message", "", 0);
            manager.sendMessage(1, message);
            //await DisplayAlert("Response", "ID:" + messageID, "OK");
        }
    }
}