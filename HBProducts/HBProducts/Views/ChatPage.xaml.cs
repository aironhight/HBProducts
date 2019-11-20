using HBProducts.Models;
using HBProducts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private async void SendRequestClicked(object sender, EventArgs e)
        {
            Message message = new Message(false, "Kurva Lapa Qko", "", 0);
            int t2Result = await manager.sendMessage(1, message);
            int tResult = await manager.GetSesionId("kurvata@gmail.com", "Kurvata Lapa");
            string t = await manager.GetSessionInfo(3);
            Session s = JsonConvert.DeserializeObject<Session>(t);
            string m = await manager.GetEmpMessages(3, 53);
            Debug.WriteLine(s);
            List<Message> list = JsonConvert.DeserializeObject<List<Message>>(m);
            await DisplayAlert("Sessio ID", "The ID is: " + s.Customer.Email, "Ok"); ;
            await DisplayAlert("Sessio ID", "The ID is: " + t2Result, "Ok");
            foreach (var i in list) {
                await DisplayAlert("Message", "The message is: " + i.Text, "Next");
                    }
        }
    }
}