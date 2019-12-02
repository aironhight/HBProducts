using HBProducts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace HBProducts.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();
            StackTrace stackTrace = new StackTrace();
            string caller = stackTrace.GetFrame(1).GetMethod().Name;

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Title="Home" },
                new HomeMenuItem {Id = MenuItemType.Products, Title="Products" },
                new HomeMenuItem {Id = MenuItemType.Scan, Title = "Scan QR"},
                new HomeMenuItem {Id = MenuItemType.FAQ, Title = "FAQ"},
                new HomeMenuItem {Id = MenuItemType.ContactUs, Title = "Contact Us"},
                new HomeMenuItem {Id = MenuItemType.About, Title="About Us" },
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return; 

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                
                await RootPage.NavigateFromMenu(id);
            };
        }

        public void setSelectedItem(int id)
        {
            if(!ListViewMenu.SelectedItem.Equals(menuItems[id]))
                ListViewMenu.SelectedItem = menuItems[id];
        }
    }
}