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

        public async void notify(string type, params object[] list)
        {
            switch(type)
            {
                case "new messages":
                    MessageListView.ScrollTo(vm.Messages.Last(), ScrollToPosition.End, false);
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