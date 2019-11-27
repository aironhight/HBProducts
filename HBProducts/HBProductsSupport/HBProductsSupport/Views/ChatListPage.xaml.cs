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
        private int empID;
        private string employeeName;
        private bool pageVisible, displayingError;


        public ChatListPage()
        {
            InitializeComponent();
            pageVisible = true;
            displayingError = false;
            manager = new ChatManager();
            this.employeeName = "Martin";
            BindingContext = viewmodel = new ChatListPageViewModel(this, employeeName);
        }


        public void notify(string type, params object[] list)
        {
            switch (type)
            {
                case "session error":
                    CallAlert("Unexpected Error", "Error while updating sessions." + Environment.NewLine + list[0].ToString());
                    break;
                case "id error":
                    CallAlert("Unexpected Error", "Error while getting employee ID" + Environment.NewLine + list[0].ToString());
                    break;
                case "no internet":
                    CallAlert("No internet connection", "There is no internet connection. The title of the page will remain the same until there is no internet connectivity...");
                    break;
            }
        }
        private void CallAlert(string title, string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if(pageVisible)
                    Alert(title, text);
            });
        }

        private async void Alert(string title, string text)
        {
            if (!displayingError)
            {
                displayingError = true;
                await DisplayAlert(title, text, "OK");
                displayingError = false;
            }
        }

        private async void onItemSelected(object sender, ItemTappedEventArgs e)
        {
            var res = await DisplayAlert("Take Session?", "Are you sure you want to take this session?", "Take", "Cancel");
            if (res)
            {
                Session s = e.Item as Session;
                int response = await manager.TakeSession(empID, s.SessionID);

                if (response != -2)
                {
                    unTakenList.SelectedItem = null;
                    if (!viewmodel.HasInternetConnection()) {
                        notify("no internet");
                        return;
                    }

                    viewmodel.IsBusy = true;
                    viewmodel.TakeSession(s.SessionID, s.Customer.Name);
                    await Navigation.PushAsync(new ChatPage(s.SessionID, s.Customer.Name));
                    viewmodel.IsBusy = false;
                }
                    
            }
        }

        private async void onTakenItemSelected(object sender, ItemTappedEventArgs e)
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