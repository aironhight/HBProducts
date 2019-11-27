using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using Newtonsoft.Json;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage, INotifyView
    {
        private ChatViewModel vm;
        private bool showingError;

        public ChatPage(int sessionID)
        {
            InitializeComponent();
            showingError = false;
            vm = new ChatViewModel(sessionID, this);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            showingError = false;
            if(vm != null)
                vm.StartUpdateRequests();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            showingError = true;
            if (vm != null)
                vm.StopUpdateRequests();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (vm.Messages != null && vm.Messages.Count > 0)
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.MakeVisible, false);
                });
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            if (vm.Messages != null && vm.Messages.Count > 0)
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
                });
            //EntryText.Focus();
        }

        private void EntryText_Focused(object sender, FocusEventArgs e)
        {
            if (vm.Messages != null && vm.Messages.Count > 0)
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
                });
        }

        public async void notify(string type, params object[] list)
        {
            switch(type)
            {
                case "new messages":
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if(vm.Messages != null && vm.Messages.Count > 0)
                            MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
                    });
                    break;

                case "error":
                    AlertMessage("Unexpected Error", "Error while getting messages: " + list[0].ToString() + Environment.NewLine + "The chat will try to update automatically.", false);
                    break;

                case "no internet":
                    AlertMessage("No internet", "The background of the page will become red and will stay red until there is no internet connectivity...", false);
                    break;

                case "message error":
                    AlertMessage("Error sending message!", "Sending the message failed too many times... The message will not be sent." + Environment.NewLine + "Error: " + list[0].ToString(), false);
                    break;

                case "session closed":
                    Device.BeginInvokeOnMainThread(() =>
                    {

                    });
                    AlertMessage("Session closed", "The session was closed. You cannot send anymore messages.", true);
                    break;

                case "EmailError":
                    AlertMessage("Error sending chat copy to the customer", list[0].ToString(), false);
                    break;

                case "send copy":
                    vm.SendChatCopy();
                    break;

                case "copy error":
                    AlertMessage("FAIL", "Error sending chat copy to the customer", false);
                    break;

                case "response":
                    Response response = (Response)list[0];

                    //Check if the enquiry was sent successfully
                    if (response.StatusCode == HttpStatusCode.Accepted)
                        AlertMessage("Sucess", "Chat copy sent successfuly", false);
                    else
                        //Alert the user if not.
                        AlertMessage("Fail", "Chat copy did not send successfuly. Error code:"+response.StatusCode.ToString(), false);
                    
                    break;

                case "session error":
                    AlertMessage("Error", "Unable to start chat session. Check your internet connectivity.", false);
                    break;

                case "error host":
                    AlertMessage("Error", "Unable to reach host after few attempts. Check your network connectivity. The app will try to reach the host again!", false);
                    break;
            }
        }

        private void AlertMessage(string title, string text, bool multipleAnswer)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Alert(title, text, multipleAnswer);
            });
        }

        private async void Alert(string title, string text, bool multipleAnswer)
        {
            if(multipleAnswer)
            {
                bool res = await DisplayAlert(title, text, "OK", "Cancel");
                if (res)
                    notify("send copy");

            }

            if(!showingError)
            {
                showingError = true;
                await DisplayAlert(title, text, "OK");
                showingError = false;
            }
        }

        private async void SendCopyClicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Send text copy", "If you click OK a copy of the current chat session will be sent to your email address immediately.", "OK", "Cancel");
            if(res)
                vm.SendChatCopy();
        }
    }
}