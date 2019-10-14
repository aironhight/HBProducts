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

        private void OpenMailClicked(object sender, EventArgs e)
        {
            OpenMail();
        }

        private async void OpenMail()
        {
            try
            {
                List<string> empty = new List<string>();
                empty.Add("aironhight@yahoo.com");
                var message = new EmailMessage
                {
                    Subject = "Test",
                    Body = "This is a test if we are genders",
                    To = empty,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }
    }
}