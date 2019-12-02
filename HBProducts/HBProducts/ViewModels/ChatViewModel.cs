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
using Xamarin.Essentials;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Http;

namespace HBProducts.ViewModels
{
    class ChatViewModel : BaseViewModel
    {
        private readonly Color internetColor = Color.White; private readonly Color noInternetColor = Color.Red;

        private Session chat;
        private ChatManager manager;
        private string textEntry;
        private int lastMessageID, sessionID, failedMessages, failedRequests;
        private INotifyView view;
        private Timer timer;
        private Color backgroundColor;
        private bool sessionClosed;

        public event EventHandler DataAdded;
        public IList<TextChatViewModel> Messages { get; set; }
        public ICommand SubmitMessageCommand { get; set; }

        public ChatViewModel(int sessionID, INotifyView view)
        {
            manager = new ChatManager();
            Messages = new ObservableCollection<TextChatViewModel>();
            this.view = view;
            this.sessionID = sessionID;
            failedMessages = 0;
            failedRequests = 0;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged; //Add an event handler for internet connectivity changes.
            SubmitMessageCommand = new Command<string>(SubmitMessage);
            textEntry = String.Empty;
            sessionClosed = false;

            setSession(sessionID);
            StartUpdateRequests();
        }
        
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { SetProperty(ref backgroundColor, value); OnPropertyChanged("BackgroundColor"); }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                BackgroundColor = internetColor;
                if (chat == null)
                    setSession(sessionID);
                else
                    StartUpdateRequests();
            }
            else
            {
                NoInternetAlert();
            }
        }

        private void NoInternetAlert()
        {
            StopUpdateRequests();
            BackgroundColor = noInternetColor;
            view.notify("no internet");
        }

        public string TextEntry
        {
            get { return textEntry; }
            set { SetProperty(ref textEntry, value); OnPropertyChanged("TextEntry"); }
        }

        private async void setSession(int sessionID)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            string chatString = await manager.GetSessionInfo(sessionID);
            this.chat = JsonConvert.DeserializeObject<Session>(chatString);
            lastMessageID = chat.GetLatestEmployeeMessageID();
            if(chat.MessageList != null) { 
                foreach (Message m in chat.MessageList) //Add all previous messages to the chat page.
                    Messages.Add(new TextChatViewModel() { Text = m.Text, Direction = m.IsEmployee ? TextChatViewModel.ChatDirection.Incoming : TextChatViewModel.ChatDirection.Outgoing });
                view.notify("new messages");
            } 
        }

        public void StartUpdateRequests()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            Task.Factory.StartNew(() =>
            {
                while (manager == null || chat == null)
                {
                    Thread.Sleep(500);
                }
                StopUpdateRequests();
                //Make the system check for new messages every 3 seconds.
                if (manager == null || chat == null) return;
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromSeconds(3);
                timer = new Timer((e) =>
                {
                    getLatestMessages();
                }, null, startTimeSpan, periodTimeSpan);
            }); 
        }

        public void StopUpdateRequests()
        {
            if(timer!=null)
                timer.Dispose();
        }

        private async void getLatestMessages()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            string jsonList = await manager.GetEmpMessages(chat.SessionID, lastMessageID);
            if (jsonList.Contains("Error"))
            {
                if (failedRequests > 5)
                {
                    if (jsonList.Substring(6).Contains("Unable to resolve host"))
                        view.notify("error host");
                    else
                        view.notify("error", jsonList.Substring(6) + Environment.NewLine + $" The messaging service has been disabled after {failedRequests} failed requests.");

                    failedRequests = 0;
                }

                failedRequests++;
                //StopUpdateRequests();
                return;
            }
            List<Message> newMesssages = JsonConvert.DeserializeObject<List<Message>>(jsonList);

            if (newMesssages.Count > 0)
            {
                lastMessageID = newMesssages[newMesssages.Count - 1].Id;
                foreach (Message m in newMesssages)
                {
                    Messages.Add(new TextChatViewModel() { Text = m.Text, Direction = TextChatViewModel.ChatDirection.Incoming });
                    chat.AddMessage(m);
                }
                   
                view.notify("new messages");
                DataAdded?.Invoke(this, null);
            }
        }

        public async void SendChatCopy()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            IsBusy = true;
            var apiKey = Constants.sendgridApiKey;
            var client = new SendGridClient(apiKey);

            string chatString = await getChatString();
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("chat-manager@hbapp.com", "ChatMessenger"),
                Subject = "HB Products chat copy",
                PlainTextContent = "Chat Copy!",
                HtmlContent = chatString
            };

            msg.AddTo(new EmailAddress(chat.Customer.Email, chat.Customer.Name));
            try
            {
                var response = await client.SendEmailAsync(msg);
                view.notify("response", response);
            }
            catch (HttpRequestException e)
            {
                view.notify("EmailError", "Error occured while sending enquiry. Check internet connection.");
            }
            catch (Exception ex)
            {
                view.notify("EmailError", "Unexpected error:" + Environment.NewLine + ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<string> getChatString()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return null;
            }
            string chatString = await manager.GetSessionInfo(sessionID);
            if (chatString.Contains("Error"))
            {
                if (chatString.Contains("Error"))
                {
                    view.notify("copy error", chatString.Substring(6));
                    return null;
                }
            }

            Session toSend = JsonConvert.DeserializeObject<Session>(chatString);

            string acc = $"<p> Customer: {toSend.Customer.Name} / {toSend.Customer.Email} <br> Employee: {toSend.Employee.Name} </p>";
            for (int i = 0; i < toSend.MessageList.Count; i++)
            {
                Message msg = toSend.MessageList[i];
                acc += "<p>";
                acc += msg.IsEmployee ? toSend.Employee.Name : toSend.Customer.Name;
                acc += ": " + msg.Text + "</p>";
            }
            return acc;
        }

        private async void SubmitMessage(string obj)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            if (manager == null || TextEntry.Length == 0) return;

            if(chat == null)
            {
                view.notify("session error");
            }

            if(sessionClosed) { view.notify("session closed"); }
            
            var x = new TextChatViewModel() { Direction = TextChatViewModel.ChatDirection.Outgoing, Text = obj };          
            int sendResult = await manager.sendMessage(chat.SessionID, new Message(false, TextEntry, "", 0));

            switch (sendResult)
            {
                case -200: //Http request returned code different from "200 OK"
                    if (failedMessages > 4)
                    {
                        view.notify("message error", "Internal server error"); //Too many failed attempts
                        return;
                    }
                    failedMessages++;
                    SubmitMessage(obj);
                    return;

                case -12: //unsuccessful tryParse in the manager.
                    if (failedMessages > 4)
                    {
                        view.notify("message error", "Server responded with an error message."); //Too many failed attempts.
                        return;
                    }
                    failedMessages++;
                    SubmitMessage(obj);
                    return;

                case -2: //Session closed
                    sessionClosed = true;
                    view.notify("session closed");
                    break;
            }

            if (failedMessages > 0)
                failedMessages = 0; //Message was sent successfully.

            chat.AddMessage(new Message(false, TextEntry, "", 0));
            Messages.Add(x);
            TextEntry = string.Empty;
            view.notify("new messages");
            DataAdded?.Invoke(this, null);

        }
    }
}
