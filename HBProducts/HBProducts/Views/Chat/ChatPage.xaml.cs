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
        private bool showingError;

        public ChatPage(int sessionID)
        {
            InitializeComponent();
            showingError = false;
            manager = new ChatManager();
            vm = new ChatViewModel(sessionID, this);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(vm != null)
                vm.StartUpdateRequests();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (vm != null)
                vm.StopUpdateRequests();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var x = MessageListView.ItemsSource;
            Device.BeginInvokeOnMainThread(() =>
            {
                MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.MakeVisible, false);
            });
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
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
                    if (!showingError)
                    {
                        showingError = true;
                        await DisplayAlert("Unexpected Error", "Error while getting messages: " + list[0].ToString() + Environment.NewLine + "The chat will try to update automatically.", "OK");
                        showingError = false;
                    }
                    break;
            }
        }
    }
}