using Android.OS;
using HBProducts.Models;
using HBProducts.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private Boolean scanned;

        public ScanPage()
        {
            InitializeComponent();
            scanned = false;
            RequestScanner();
        }

        private async void RequestScanner()
         {
             Debug.WriteLine("I AM WORKING BRO!");
             try
             {
                 var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                System.Diagnostics.Debug.WriteLine("Permission status: " + status.ToString());
                 if (status != PermissionStatus.Granted)
                 {
                     if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                     {
                         await DisplayAlert("Need Camera", "In order to scan we will need camera permissions", "OK");
                     }

                     status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                 }

                 if (status == PermissionStatus.Granted)
                 {
                     //Permission granted
                     StartScanner();
                 }
                 else if (status != PermissionStatus.Unknown)
                 {
<
                    //Permission denied
                    switch(Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            await DisplayAlert("Camera permissions denied!", "In order to scan we will need camera permissions. Turn them on from the settings page of our app.", "OK");
                            break;
                    }
                 }
             }
             catch (Exception ex)
             {
                //Something went wrong
                System.Diagnostics.Debug.WriteLine(ex.Message);
             }
         }

        public async void StartScanner()
        {
            var ScannerPage = new ZXingScannerPage();
            ScannerPage.IsTorchOn = true;

            ScannerPage.OnScanResult += (result) =>
            {
                // Finish scanning
                ScannerPage.IsScanning = false;
                
                int scannedId = -1;

                if(int.TryParse(result.Text, out scannedId))
                {
                    System.Diagnostics.Debug.WriteLine("Good QR code.");
                    //If the scanned code contains ID.
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //Navigation.PopAsync();
                        openProductPage(scannedId);
                    });
                    
                    return;
                } else {
                    System.Diagnostics.Debug.WriteLine("Bad QR code");
                    //Product not recognized!  
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopAsync();
                        DisplayAlert("ERROR!", "Product not recognized. Please try again.", "OK");
                    });
                    return;
                }
            };
                
            await Navigation.PushAsync(ScannerPage);
        }

        private async void openProductPage(int id)
        {
            ProductsViewModel vm = new ProductsViewModel();
            Product scannedProduct = await vm.GetProductWithId(id);
            Device.BeginInvokeOnMainThread(() =>
            {
                //Check if the product is not null
                if (scannedProduct != null) {
                    //If its not - start the product page
                    //Navigation.PopAsync();
                    startProductPage(scannedProduct);
                } else
                {
                    //Alert the user that the product couldnt be loaded.
                    Navigation.PopAsync();
                    DisplayAlert("Error", "Product couldn't be loaded. Check your internet connection.", "OK");
                }
            });
        }

        private async void startProductPage(Product prod)
        {
            var networkAccess = Connectivity.NetworkAccess; //Check internet connectivity
            if (networkAccess == NetworkAccess.Internet)
            {
                if (!scanned)
                {
                    try { 
                        scanned = true;
                        //Navigation.RemovePage(this);
                        await Navigation.PopAsync();
                        await Navigation.PushModalAsync(new ProductPage(prod));
                    } catch (Exception ex)
                    {
                        await Navigation.PopAsync();
                        await DisplayAlert("Error", ex.Message, "OK");
                    }
                }
            } else
            {

                await Navigation.PopAsync();
                await DisplayAlert("No internet access", "In order to get data for products the app needs internet access.", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            scanned = false;
        }

        private void ScanButtonClicked(object sender, EventArgs e)
        {
            RequestScanner();
        }
    }
}
