using HBProducts.Views.Chat;
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
            await Navigation.PushAsync(new ChatEntryFields());
        }

        private void SendEmailButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EnquiryPage());
        }

        private async void CallButtonClicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(new Uri("tel:" + Constants.telephone));
        }

        protected override bool OnBackButtonPressed()
        {
            var mdp = Xamarin.Forms.Application.Current.MainPage as MainPage;
            mdp.NavigateToPage(0);
            return true;
        }
    }
}