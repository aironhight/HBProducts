using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using HBProducts.Models;

namespace HBProducts.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Home, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.Products:
                        MenuPages.Add(id, new NavigationPage(new ProductsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.ContactUs:
                        MenuPages.Add(id, new NavigationPage(new ContactUsPage()));
                        break;
                    case (int)MenuItemType.FAQ:
                        MenuPages.Add(id, new NavigationPage(new FAQPage()));
                        break;
                    case (int)MenuItemType.Scan:
                        MenuPages.Add(id, new NavigationPage(new ScanPage()));
                        break;
                }
                
            }  

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }

            
            //
        }
        public void NavigateToPage(int id)
        {
            menu.setSelectedItem(id);
        }
    }
}