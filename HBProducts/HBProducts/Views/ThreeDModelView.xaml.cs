
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Urho;
using Urho.Forms;

namespace HBProducts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ThreeDModelView : ContentPage
    {

        UrhoSurface urhoSurface;
        public ThreeDModelViewer shit;

        public ThreeDModelView()
        {
            InitializeComponent();
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            Content = new StackLayout
            {
                Padding = new Thickness(12, 12, 12, 40),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
      urhoSurface }
            };
        }
        protected override async void OnAppearing()
        {

            //urhoSurface = new UrhoSurface();
            //if(shit == null) {

            (Xamarin.Forms.Application.Current.MainPage as MainPage).IsGestureEnabled = false;

            shit = await urhoSurface.Show<ThreeDModelViewer>(new ApplicationOptions("Materials")
            { Orientation = ApplicationOptions.OrientationType.Portrait });

        }

        protected override async void OnDisappearing()
        {
            (Xamarin.Forms.Application.Current.MainPage as MainPage).IsGestureEnabled = true;

        }
    }
}