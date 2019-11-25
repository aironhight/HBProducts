using HBProducts.Models;
using HBProducts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace HBProducts.ViewModels
{
    class ChatViewModel : BaseViewModel
    {

        private Session chat;
        private ICommand OnSendCommand;
        // private Boolean isLoading, noInternetConnection;
        private ChatManager manager;

        public ChatViewModel(int sessionID)
        {
            manager = new ChatManager();
            chat = new Session("test", new System.Collections.ObjectModel.ObservableCollection<Message>(), new Customer("Ivo", "HB", "emailGmail.com", "123123", "DK"), new Employee("Gosho"), 1);
            //setSession(sessionID);

            chat.AddMessage(new Message(false, "Hi!", "", 0));
            chat.AddMessage(new Message(false, "How is it?", "", 1));

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextMessage))
                {
                    chat.AddMessage(new Message(false, TextMessage, "", 0));
                    TextMessage = string.Empty;
                }
            });
        }

        private async void setSession(int sessionID)
        {
            string chatString = await manager.GetSessionInfo(sessionID);
            this.chat = JsonConvert.DeserializeObject<Session>(chatString);
        }

        public string TextMessage { get; set; }
    }
}
