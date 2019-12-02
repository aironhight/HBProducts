using HBProducts.Models;
using HBProducts.Services;
using HBProducts.Views;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace HBProducts.ViewModels
{
    class ProductViewModel : BaseViewModel
    {
        private readonly string markLabel = "Mark as Favorite"; private readonly string removeLabel = "Remove from Favorites";

        private Product product;
        private INotifyView view;
        private string favoriteLabelText;
        

        public ICommand URLClicked{ get { return new Command<string>((url) => urlClicked(url)); } }

        public ICommand AddToFavorites { get { return new Command(FavoritesCommandCalled); } }

        public ProductViewModel(Product product, INotifyView view)
        {
            this.product = product;
            this.view = view;
            FavoriteLabelText = Settings.ProductIsFavorite(product) ? removeLabel : markLabel;
        }

        public Product Product
        {
            set { SetProperty(ref product, value); }
            get { return product; }
        }

        private void FavoritesCommandCalled()
        {
            //Check if product is in favorites...
            if (Settings.ProductIsFavorite(product)) {
                Settings.RemoveProductFromFavorites(product); //Remove it if it is
                FavoriteLabelText = markLabel;
                view.notify("unfavorited");
            }
            else { 
                Settings.AddToFavorites(product); //Add it if it isnt
                FavoriteLabelText = removeLabel;
                view.notify("favorited");
            }
        }

        public String FavoriteLabelText
        {
            get { return favoriteLabelText; }
            set { SetProperty(ref favoriteLabelText, value); OnPropertyChanged(FavoriteLabelText); }
        }

        public async void urlClicked(string url)
        {
            Debug.WriteLine("URL CLICKED: " + url);

            Device.OpenUri(new Uri(url));

            //Code for opening in the in-app web viewer.
            //switch (Device.RuntimePlatform)
            //{
            //    case Device.iOS:
            //        view.notify("openWebViewer", url);
            //        break;

            //    default:
            //        Device.OpenUri(new Uri(url));
            //        break;
            //}
        }

    }
}
