using HBProducts.Views.Tabs.Enquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUsPage : ContentPage
    {
        public ContactUsPage()
        {
            InitializeComponent();
        }

        private void OpenChatButtonClicked(object sender, EventArgs e)
        {
            OpenChat();
        }

        private async void OpenChat()
        {
            await Navigation.PushAsync(new ChatPage());
        }

        private void SendEmailButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnquiryPage());
        }

        private async void CallButtonClicked(object sender, EventArgs e)
        {
           // Device.OpenUri(new Uri("tel:038773729"));
            //Launcher.OpenUri(new Uri("tel:038773729"));
            await Launcher.OpenAsync(new Uri("tel:038773729"));
        }
    }
}