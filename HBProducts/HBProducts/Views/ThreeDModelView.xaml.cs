
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
        public ThreeDModelViewer modelViewer;
        private string threeDModel;

        public ThreeDModelView(string threeDModel)
        {
            InitializeComponent();
            this.threeDModel = threeDModel;
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
            (Xamarin.Forms.Application.Current.MainPage as MainPage).IsGestureEnabled = false;

            modelViewer = await urhoSurface.Show<ThreeDModelViewer>(new ApplicationOptions("Materials")
            { Orientation = ApplicationOptions.OrientationType.Portrait });

            modelViewer.setThreeDModelName(threeDModel);
            Urho.Application.InvokeOnMain(()=>modelViewer.startDisplaying());
        }

        protected override async void OnDisappearing()
        {
            (Xamarin.Forms.Application.Current.MainPage as MainPage).IsGestureEnabled = true;

        }
    }
}