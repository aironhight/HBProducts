using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;

using Plugin.AzurePushNotification;
using Android.Content;

namespace HBProducts.Droid
{

    [Activity(Label = "HBProducts", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;


            CrossCurrentActivity.Current.Activity = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            AzurePushNotificationManager.ProcessIntent(this, Intent);

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                AzurePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                //Change for your default notification channel name here
                AzurePushNotificationManager.DefaultNotificationChannelName = "General";
            }

            #if DEBUG
                        AzurePushNotificationManager.Initialize(this, Constants.ListenConnectionString, Constants.NotificationHubName, true);
            #else
                             AzurePushNotificationManager.Initialize(this, AzureConstants.ListenConnectionString, AzureConstants.NotificationHubName, false);
            #endif


            //Handle notification when app is closed here
            CrossAzurePushNotification.Current.OnNotificationReceived += (s, p) =>
            {


            };
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            AzurePushNotificationManager.ProcessIntent(this, intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}