using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.AzurePushNotification;
using Plugin.CurrentActivity;

namespace HBProducts.Droid
{
#if DEBUG
    [Application(Debuggable = true)]
#else
	[Application(Debuggable = false)]
#endif
    public class MainApplication : Application
    {
        protected MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

       public override void OnCreate()
		{
			base.OnCreate();
			CrossCurrentActivity.Current.Init(this);

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                AzurePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                //Change for your default notification channel name here
                AzurePushNotificationManager.DefaultNotificationChannelName = "General";
            }

#if DEBUG
            AzurePushNotificationManager.Initialize(this, Constants.ListenConnectionString,Constants.NotificationHubName, true);
#else
                 AzurePushNotificationManager.Initialize(this, Constants.ListenConnectionString, Constants.NotificationHubName, false);
#endif

            CrossAzurePushNotification.Current.RegisterAsync(new string[] { "android", "general" });
            //Handle notification when app is closed here
            CrossAzurePushNotification.Current.OnNotificationReceived += (s, p) =>
            {


            };
        }


    }
}
