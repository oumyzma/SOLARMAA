namespace SOLARMAA
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnClicked(object sender, EventArgs e)
        {
            ProgressBar progressBar = this.FindByName<ProgressBar>("myProgressBar");
            double currentValue = progressBar.Progress;
            currentValue += 0.1;
            progressBar.Progress = currentValue;
        }
    }
}