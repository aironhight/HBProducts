using HBProductsSupport.Models;
using HBProductsSupport.Services;
using HBProductsSupport.ViewModels;
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

namespace HBProductsSupport.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage, INotifyView
    {
        private ChatViewModel vm;
        private bool displayingError;

        public ChatPage(int sessionID, string username)
        {
            InitializeComponent();
            displayingError = false;
            vm = new ChatViewModel(sessionID, this);
            BindingContext = vm;
            Title = username;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            displayingError = false;
            vm.StartUpdateRequests();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            displayingError = true;
            vm.StopUpdateRequests();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var x = MessageListView.ItemsSource;
            MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.MakeVisible, false);
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() => {
                MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
            });
            //EntryText.Focus();
        }

        private void EntryText_Focused(object sender, FocusEventArgs e)
        {
            if (vm.Messages.Count != 0)
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
                    AlertMessage("Unexpected Error", "Error while getting messages: " + list[0].ToString() + Environment.NewLine + "The chat will try to update automatically.");
                    break;

                case "session error":
                    AlertMessage("Session Error", "Error while getting session info: " + list[0].ToString() + Environment.NewLine + "The session will try to update automatically.");
                    break;

                case "EmailError":
                    AlertMessage("Error sending chat copy to the customer", list[0].ToString());
                    break;

                case "closing error":
                    AlertMessage("Error closing the session", list[0].ToString());
                    break;

                case "session closed":
                    await Navigation.PopAsync();
                    break;

                case "message error":
                    AlertMessage("Error sending message!", "Sending the message failed too many times... The message will not be sent." + Environment.NewLine + "Error: " + list[0].ToString());
                    break;

                case "closed without copy":
                    AlertMessage("Error sending a copy", "The copy failed to send.");
                    break;

                case "no internet":
                    AlertMessage("No internet","There is no internet connection. The background of the chat will stay red until there is no internet connectivity");
                    break;

                case "no session error":
                    AlertMessage("Error", "Unable to start chat session. Check your internet connectivity.");
                    break;

                case "error host":
                    AlertMessage("Error", "Unable to reach host after few attempts. Check your network connectivity. The app will try to reach the host again!");
                    break;
            }
        }

        private void AlertMessage(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() => Alert(title, message));
        }

        private async void Alert(string title, string message)
        {
            if (!displayingError)
            {
                displayingError = true;
                await DisplayAlert(title, message, "OK");
                displayingError = false;
            }
        }

        private async void closeSessionClicked(object sender, EventArgs e)
        {
            if(!vm.HasInternetEnabled())
            {
                AlertMessage("No internet", "You cannot close a session if there are no internet services.");
                return;
            }
            var res = await DisplayAlert("Close session?", "Are you sure that you want to close this session?", "Continue", "Cancel");
            if (res)
            {
                var sendRes = await DisplayAlert($"Send copy to email?", "Do you want to send a copy of the chat to " + Constants.supportEmail + "?", "Yes", "Cancel");
                vm.CloseSessionAsync(sendRes);
            }
        }

        private void infoClicked(object sender, EventArgs e)
        {
            DisplayAlert("Customer info", 
                "Name: " + vm.GetCustomerName() + Environment.NewLine +
                "Email: " + vm.GetCustomerEmail(), "OK");
        }
    }
}