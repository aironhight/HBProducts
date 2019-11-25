using HBProducts.Models;
using HBProducts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;

namespace HBProducts.ViewModels
{
    class ChatViewModel : BaseViewModel
    {

        private Session chat;
        private ICommand OnSendCommand;
        // private Boolean isLoading, noInternetConnection;
        private ChatManager manager;
        private string textEntry;
        private int lastMessageID;
        private INotifyView view;

        public event EventHandler DataAdded;
        public IList<TextChatViewModel> Messages { get; set; }
        public ICommand SubmitMessageCommand { get; set; }

        public ChatViewModel(int sessionID, INotifyView view)
        {
            manager = new ChatManager();
            setSession(sessionID);
            Messages = new ObservableCollection<TextChatViewModel>();
            this.view = view;
            
            SubmitMessageCommand = new Command<string>(SubmitMessage);
            textEntry = String.Empty;

            OnSendCommand = new Command(() =>
            {
                if (!string.IsNullOrEmpty(TextMessage))
                {
                    chat.AddMessage(new Message(false, TextMessage, "", 0));
                    TextMessage = string.Empty;
                }
            });
        }

        public string TextEntry
        {
            get { return textEntry; }
            set { SetProperty(ref textEntry, value); OnPropertyChanged("TextEntry"); }
        }

        private async void setSession(int sessionID)
        {
            string chatString = await manager.GetSessionInfo(sessionID);
            this.chat = JsonConvert.DeserializeObject<Session>(chatString);
            lastMessageID = chat.GetLatestEmployeeMessageID();
            foreach (Message m in chat.MessageList) //Add all previous messages to the chat page.
                Messages.Add(new TextChatViewModel() { Text = m.Text, Direction = m.IsEmployee ? TextChatViewModel.ChatDirection.Incoming : TextChatViewModel.ChatDirection.Outgoing });

            //Make the system check for new messages every 3 seconds.
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(3);
            var timer = new Timer((e) =>
            {
                getLatestMessages();
            }, null, startTimeSpan, periodTimeSpan);
        }

        private async void getLatestMessages()
        {
            string jsonList = await manager.GetEmpMessages(chat.SessionID, lastMessageID);
            if(jsonList.Contains("Error:"))
            {
                //TO BE IMPLEMENTED!!!
                return;
            }
            List<Message> newMesssages = JsonConvert.DeserializeObject<List<Message>>(jsonList);
            
            if(newMesssages.Count > 0) {
                lastMessageID = newMesssages[newMesssages.Count - 1].Id;
                foreach (Message m in newMesssages) 
                    Messages.Add(new TextChatViewModel() { Text = m.Text, Direction = TextChatViewModel.ChatDirection.Incoming });

                view.notify("new messages");
                DataAdded?.Invoke(this, null);
            }
        }

        public string TextMessage { get; set; }

        private void SubmitMessage(string obj)
        {
            var x = new TextChatViewModel() { Direction = TextChatViewModel.ChatDirection.Outgoing, Text = obj };
            Messages.Add(x);
            
            manager.sendMessage(chat.SessionID, new Message(false, TextEntry, "", 0));
            TextEntry = string.Empty;
            view.notify("new messages");
            DataAdded?.Invoke(this, null);
        }
    }
}
