using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
            RequestScanner();
        }

        private async void RequestScanner()
        {
            Debug.WriteLine("I AM WORKING BRO!");
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                Debug.WriteLine("Permission status: " + status.ToString());
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
                    //Camera permission denied...
                }
            }
            catch (Exception ex)
            {
                //Something went wrong
            }

        }



        public async void StartScanner()
        {
            var ScannerPage = new ZXingScannerPage();

            ScannerPage.OnScanResult += (result) =>
            {
                // Finish scanning
                ScannerPage.IsScanning = false;

                // Pop-up
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    DisplayAlert("You scanned ", result.Text, "OK");
                });
            };


            await Navigation.PushAsync(ScannerPage);
        }

        private void ScanButtonClicked(object sender, EventArgs e)
        {
            RequestScanner();
        }
    }
}