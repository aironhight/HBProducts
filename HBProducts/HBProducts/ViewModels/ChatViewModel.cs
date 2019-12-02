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

            SetSession(sessionID);
            StartUpdateRequests();
        }
        
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { SetProperty(ref backgroundColor, value); OnPropertyChanged("BackgroundColor"); }
        }

        //Connectivity changed event handler
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                BackgroundColor = internetColor;
                if (chat == null)
                    SetSession(sessionID);
                else
                    StartUpdateRequests();
            }
            else
            {
                NoInternetAlert();
            }
        }

        //Stops the update requests and alerts the user for no internet services enabled
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

        //Requests the session object from the server(in case there are any messages already in it)

        private async void SetSession(int sessionID)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            string chatString = await manager.GetSessionInfo(sessionID);
            this.chat = JsonConvert.DeserializeObject<Session>(chatString);
            lastMessageID = chat.GetLatestEmployeeMessageID(); //Update the latest message id so that the message does not appear multiple times.

            if (chat.MessageList != null) { 
                foreach (Message m in chat.MessageList) //Add all previous messages to the chat page.
                    Messages.Add(new TextChatViewModel() { Text = m.Text, Direction = m.IsEmployee ? TextChatViewModel.ChatDirection.Incoming : TextChatViewModel.ChatDirection.Outgoing });
                view.notify("new messages"); //Notify the view so that it scrolls to the latest message in the chat session.
            } 
        }

        //Starts the timer for updating the chat message list.
        public void StartUpdateRequests()
        {
            //Check for internet connection
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            //Starts a timer on a new thread which makes requests every 3 seconds.
            Task.Factory.StartNew(() =>
            {
                //While the chat manager and chat session are null - put the thread to sleep.
                while (manager == null || chat == null)
                {
                    Thread.Sleep(500);
                }
                StopUpdateRequests(); //Stop the timer to prevent multiple timers running at once

                //Start a timer which requests new messages every 3 seconds.
                if (manager == null || chat == null) return;
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromSeconds(3);
                timer = new Timer((e) =>
                {
                    GetLatestMessages();
                }, null, startTimeSpan, periodTimeSpan);
            }); 
        }

        //Stops the timer for updating the chat message list.
        public void StopUpdateRequests()
        {
            if(timer!=null)
                timer.Dispose();
        }

       //Gets the messages sent after the latest received message.
        private async void GetLatestMessages()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                NoInternetAlert();
                return;
            }
            
            string jsonMessageList = await manager.GetEmpMessages(chat.SessionID, lastMessageID); //Get the employee messages in a json list.

            //Check if the response contains error.
            if (jsonMessageList.Contains("Error")) 
            {
                if (failedRequests > 5) //If too many errors have occured
                {
                    if (jsonMessageList.Substring(6).Contains("Unable to resolve host"))
                        view.notify("error host"); //Can't reach host exception
                    else
                        view.notify("error", jsonMessageList.Substring(6) + Environment.NewLine + $" The messaging service has encountered {failedRequests} failed requests.");

                    failedRequests = 0;
                }

                failedRequests++;
                return;
            }

            //If no errors have occured - convert the json message list from a string to list.
            List<Message> newMesssages = JsonConvert.DeserializeObject<List<Message>>(jsonMessageList);

            if (newMesssages.Count > 0) //Check if the list isn't empty
            {
                lastMessageID = newMesssages[newMesssages.Count - 1].Id; 
                foreach (Message m in newMesssages) //Add the messages in the chat list.
                {
                    Messages.Add(new TextChatViewModel() { Text = m.Text, Direction = TextChatViewModel.ChatDirection.Incoming });
                    chat.AddMessage(m);
                }
                   
                view.notify("new messages"); //Notify the view for new messages in order to scroll to the last message.
                DataAdded?.Invoke(this, null);
            }
        }

        //Sends a chat copy to the customer's email address.
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
                view.notify("email response", response);
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

        //Requests the current session from the server and makes it into a string.
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

        //Submits a message in the chat
        private async void SubmitMessage(string messageText)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet) {
                NoInternetAlert();
                return;
            }
            if (manager == null || TextEntry.Length == 0) return;

            if(chat == null) {
                view.notify("session error");
                return;
            }

            if(sessionClosed) {
                view.notify("session closed"); return;
            }
            
            var messageBubble = new TextChatViewModel() { Direction = TextChatViewModel.ChatDirection.Outgoing, Text = messageText };          
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
                    SubmitMessage(messageText); //make another attempt to send the message.
                    return;

                case -12: //unsuccessful tryParse in the manager.
                    if (failedMessages > 4)
                    {
                        view.notify("message error", "Server responded with an error message."); //Too many failed attempts.
                        return;
                    }
                    failedMessages++;
                    SubmitMessage(messageText); //Make another attempt to send the message.
                    return;

                case -2: //Session closed
                    sessionClosed = true;
                    view.notify("session closed");
                    break;
            }

            if (failedMessages > 0)
                failedMessages = 0; //Message was sent successfully.

            chat.AddMessage(new Message(false, TextEntry, "", 0));
            Messages.Add(messageBubble); //Add the message blob to the list
            TextEntry = string.Empty; //Empty the entry field
            view.notify("new messages"); //notify the view to scroll down
            DataAdded?.Invoke(this, null);
        }
    }
}
