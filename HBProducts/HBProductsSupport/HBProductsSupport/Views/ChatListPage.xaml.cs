using HBProductsSupport.Models;
using HBProductsSupport.Services;
using HBProductsSupport.ViewModels;
using HBProductsSupport.Views.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProductsSupport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatListPage : ContentPage, INotifyView
    {
        private ChatListPageViewModel viewmodel;
        private ChatManager manager;
        private string employeeName;
        private bool pageVisible, displayingError;

        public ChatListPage()
        {
            InitializeComponent();
            pageVisible = true;
            displayingError = false;
            manager = new ChatManager();
            this.employeeName = "Mathias";
            BindingContext = viewmodel = new ChatListPageViewModel(this, employeeName);
        }

        //Notification handler.
        public void notify(string type, params object[] list)
        {
            switch (type)
            {
                case "session error":
                    CallAlert("Unexpected Error", "Error while updating sessions." + Environment.NewLine + list[0].ToString());
                    break;
                case "id error":
                    CallAlert("Unexpected Error", "Error while trying to get the employee ID from the API. Check internet connection...");
                    break;
                case "no internet":
                    CallAlert("No internet connection", "There is no internet connection. The title of the page will remain the same until there is no internet connectivity...");
                    break;
                case "session parsing error":
                    CallAlert("Error while taking session.", "Received unexpected answer.");
                    break;
                case "session already taken":
                    CallAlert("Error while taking session.", "Session already taken.");
                    break;
                case "session nonexist":
                    CallAlert("Error while taking session.", "Session does not exist.");
                    break;

            }
        }

        //Makes a Alert call on the UI thread.
        private void CallAlert(string title, string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if(pageVisible)
                    Alert(title, text);
            });
        }

        //Creates an alert message on the screen.
        private async void Alert(string title, string text)
        {
            if (!displayingError)
            {
                displayingError = true;
                await DisplayAlert(title, text, "OK");
                displayingError = false;
            }
        }

        //On unanswered session click
        private async void OnUnansweredSessionSelected(object sender, ItemTappedEventArgs e)
        {
            var res = await DisplayAlert("Take Session?", "Are you sure you want to take this session?", "Take", "Cancel");
            if (res)
            {
                //Check if there is internet connection
                if (!viewmodel.HasInternetConnection())
                {
                    notify("no internet");
                    return;
                }

                Session s = e.Item as Session;

                unTakenList.SelectedItem = null; //Unmark the selected item from the UI
                
                viewmodel.IsBusy = true;
                //Make a request and await for a response if the session was successfuly taken.
                bool sessionTaken = await viewmodel.TakeSession(s.SessionID, s.Customer.Name);
                //If the session was successfuly taken - Open a new page with the session.
                if(sessionTaken)
                    await Navigation.PushAsync(new ChatPage(s.SessionID, s.Customer.Name));

                viewmodel.IsBusy = false;

                    
            }
            unTakenList.SelectedItem = null;
        }

        //Called on Taken session selected.
        private async void OnTakenSessionSelected(object sender, ItemTappedEventArgs e)
        {
            {
                takenList.SelectedItem = null;
                if (!viewmodel.HasInternetConnection()) {
                    notify("no internet");
                    return;
                }

                viewmodel.IsBusy = true;
                Session s = e.Item as Session;
                await Navigation.PushAsync(new ChatPage(s.SessionID, s.Customer.Name));
                viewmodel.IsBusy = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageVisible = true;
            displayingError = false;
            if(viewmodel != null)
                viewmodel.StartUpdatingChatList();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageVisible = false;
            displayingError = true;
            if (viewmodel != null)
                viewmodel.StopUpdatingChatList();
        }
    }
}