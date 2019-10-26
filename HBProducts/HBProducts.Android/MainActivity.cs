using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Urho.Droid;


namespace HBProducts.Droid
{

    [Activity(Label = "HBProducts", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //UrhoSurfacePlaceholder surface;
        //ThreeDModelViewer game;
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            
            CrossCurrentActivity.Current.Activity = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            //    var mLayout = new RelativeLayout(this);
            //    surface = UrhoSurface.CreateSurface(this);// (this, , true);
            //    mLayout.AddView(surface);
            //    SetContentView(mLayout);
            //    game = await surface.Show<ThreeDModelViewer>(new Urho.ApplicationOptions("Data"));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        //protected override void OnResume()
        //{
        //    UrhoSurface.OnResume();
        //    base.OnResume();
        //}

        //protected override void OnPause()
        //{
        //    UrhoSurface.OnPause();
        //    base.OnPause();
        //}

        //public override void OnLowMemory()
        //{
        //    UrhoSurface.OnLowMemory();
        //    base.OnLowMemory();
        //}

        //protected override void OnDestroy()
        //{
        //    UrhoSurface.OnDestroy();
        //    base.OnDestroy();
        //}

        //public override bool DispatchKeyEvent(KeyEvent e)
        //{
        //    if (e.KeyCode == Android.Views.Keycode.Back)
        //    {
        //        this.Finish();
        //        return false;
        //    }

        //    return base.DispatchKeyEvent(e);
        //}

        //public override void OnWindowFocusChanged(bool hasFocus)
        //{
        //    UrhoSurface.OnWindowFocusChanged(hasFocus);
        //    base.OnWindowFocusChanged(hasFocus);
        //}

    }
}