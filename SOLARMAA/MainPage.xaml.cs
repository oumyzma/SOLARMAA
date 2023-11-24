namespace SOLARMAA
{
    public partial class MainPage : ContentPage
    {
        int count = 0;


        public MainPage()
        {
            InitializeComponent();
            
            OrientationSensor.ReadingChanged += OnOrientationSensorReadingChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Gps _gps = new Gps();
            var a = await _gps.GetCurrentLocation();
            await DisplayAlert("alert", a, "OK");
        }

    }
}