using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage, INotifyView
    {
        private ChatManager manager;
        private ChatViewModel vm;
        private int sessionID;

        public ChatPage(int sessionID)
        {
            InitializeComponent();
            manager = new ChatManager();
            vm = new ChatViewModel(sessionID, this);
            BindingContext = vm;   
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }



        private void Button_Clicked(object sender, EventArgs e)
        {
            var x = MessageListView.ItemsSource;
            MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.MakeVisible, false);
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
            //EntryText.Focus();
        }

        private void EntryText_Focused(object sender, FocusEventArgs e)
        {
            if(vm.Messages.Count != 0) 
                MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
        }

        public void notify(string type, params object[] list)
        {
            switch(type)
            {
                case "new messages":
                    MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
                    break;
            }
        }
    }
}