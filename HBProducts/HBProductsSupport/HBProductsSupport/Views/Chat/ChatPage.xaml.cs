using HBProductsSupport.Models;
using HBProductsSupport.Services;
using HBProductsSupport.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProductsSupport.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage, INotifyView
    {
        private ChatManager manager;
        private ChatViewModel vm;
        private int sessionID;
        private bool showingError;

        public ChatPage(int sessionID, string username)
        {
            InitializeComponent();
            showingError = false;
            manager = new ChatManager();
            vm = new ChatViewModel(sessionID, this);
            BindingContext = vm;
            Title = username;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.StartUpdateRequests();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
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
                        MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
                    });
                    break;

                case "error":
                    Device.BeginInvokeOnMainThread(() => { displayAlert("Unexpecter error", list[0].ToString() + Environment.NewLine + "The chat will try to update automatically.");    });
                    break;
            }
        }

        private async void displayAlert(string title, string message)
        {
            showingError = true;           
            await DisplayAlert(title, message, "OK");
            showingError = false;
        }
    }
}