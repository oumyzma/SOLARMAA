using SOLARMAA.Services;

namespace SOLARMAA
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

        }

        private async void CounterBtn_Clicked(object sender, EventArgs e)
        {
            Gps _gps = new Gps();
            var a = await _gps.GetCachedLocation();
            await DisplayAlert("alert", a, "OK");

            ProgressBar progressBar = this.FindByName<ProgressBar>("myProgressBar");
            double currentValue = progressBar.Progress;
            currentValue += 0.1;
            progressBar.Progress = currentValue;
        }
    }
}