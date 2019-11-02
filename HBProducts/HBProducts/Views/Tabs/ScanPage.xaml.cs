﻿using Android.OS;
using HBProducts.Models;
using HBProducts.Services;
using HBProducts.ViewModels;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
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
        private ProductManager manager;
        

        public ScanPage()
        {
            InitializeComponent();
            scanned = false;
            manager = new ProductManager();
            RequestScanner();
        }

        private async void RequestScanner()
         {
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
                        getProductWithId(scannedId);
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

        private async void getProductWithId(int id)
        {
            string productString = await manager.getProductWithId(id);

            Device.BeginInvokeOnMainThread(() =>
            {
                if (productString.Contains("Error:")) {
                    Navigation.PopAsync();
                    DisplayAlert("Error", productString.Substring(6), "OK");
                    return;
                } else {
                    Product scannedProduct = JsonConvert.DeserializeObject<Product>(productString);
                    startProductPage(scannedProduct);
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