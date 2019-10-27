using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HBProducts.Views.iOS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewerPage : ContentPage
    {
        public WebViewerPage(string url)
        {
            InitializeComponent();
            webView.Source = url;
        }
    }
}