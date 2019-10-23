using System.ComponentModel;
using Xamarin.Forms;
using Urho;
using Urho.Forms;

namespace HBProducts.Views
{
    public partial class HomePage : ContentPage

    {
        //UrhoSurface urhoSurface;
        //public ThreeDModelViewer shit;
        //bool isInit = false;

        public HomePage()
        {
            InitializeComponent();
            //      bool isInit;
            //      urhoSurface = new UrhoSurface();
            //      urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            //      Content = new StackLayout
            //      {
            //          Padding = new Thickness(12, 12, 12, 40),
            //          VerticalOptions = LayoutOptions.FillAndExpand,
            //          Children = {
            //urhoSurface }
            //      };

            //webView.Source = "https://www.hbproducts.dk/en/hb-products-link/news-archive.html";


        }



        //protected override async void OnAppearing()
        //{

        //    //urhoSurface = new UrhoSurface();
        //    //if(shit == null) {

        //    (Xamarin.Forms.Application.Current.MainPage as MainPage).IsGestureEnabled = false;

        //    shit = await urhoSurface.Show<ThreeDModelViewer>(new ApplicationOptions("Materials")
        //    { Orientation = ApplicationOptions.OrientationType.Portrait });
        //    //}
        //    //else
        //    //{

        //    //    await urhoSurface.Show<ThreeDModelViewer>(new ApplicationOptions("Materials")
        //    //    { Orientation = ApplicationOptions.OrientationType.Portrait });
        //    //}
        //}



        //protected override async void OnDisappearing()
        //{
        //   UrhoSurface.OnPause();

        //}


        //public async void OnSleep()
        //{
        //    // await urhoSurface.Stop();
        //    //shit.Stop();
        //    await shit.Exit();
        //}

        async void StartUrho()
        {

            await Navigation.PushAsync(new ThreeDModelView());

        }

        void StartScene(object sender, System.EventArgs e)
        {
            StartUrho();
        }
    }

}
