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

        private void OpenChatClicked(object sender, EventArgs e)
        {
            OpenChat();
        }

        private async void OpenChat()
        {
            await Navigation.PushAsync(new ChatPage());
        }
    }
}