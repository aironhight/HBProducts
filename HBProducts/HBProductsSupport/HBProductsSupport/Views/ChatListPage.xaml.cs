using HBProductsSupport.Services;
using HBProductsSupport.ViewModels;
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

        public ChatListPage()
        {
            InitializeComponent();
            BindingContext = viewmodel = new ChatListPageViewModel(this);

        }

        public void notify(string type, params object[] list)
        {
            throw new NotImplementedException();
        }
    }
}