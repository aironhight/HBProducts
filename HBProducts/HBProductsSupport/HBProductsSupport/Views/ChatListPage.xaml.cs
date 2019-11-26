using HBProductsSupport.Models;
using HBProductsSupport.Services;
using HBProductsSupport.ViewModels;
using HBProductsSupport.Views.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ChatListPage()
        {
            InitializeComponent();
            manager = new ChatManager();
            setViewModel("Martin");
          
        }

        public void notify(string type, params object[] list)
        {
            //throw new NotImplementedException();
        }

        private async void setViewModel(string name)
        {
            empID = await manager.GetEmpID(name);
            BindingContext = viewmodel = new ChatListPageViewModel(this, empID);
        }

        private async void onItemSelected(object sender, ItemTappedEventArgs e)
        {
            var res = await DisplayAlert("Take Session?", "Are you sure you want to take this session?", "Take", "Cancel");
            if (res)
            {
                Session s = e.Item as Session;
                int response = await manager.TakeSession(empID, s.SessionID);

                if (response !=-2)
                    await Navigation.PushAsync(new ChatPage(s.SessionID, s.Customer.Name));
            }
        }

        private async void onTakenItemSelected(object sender, ItemTappedEventArgs e)
        {
            {
                Session s = e.Item as Session;
                await Navigation.PushAsync(new ChatPage(s.SessionID, s.Customer.Name));
            }
        }
    }
}