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

        private void OnClicked(object sender, EventArgs e)
        {
            Gps _gps = new Gps();
            var a = await _gps.GetCachedLocation();
            await DisplayAlert("alert", a, "OK");

            ProgressBar progressBar = this.FindByName<ProgressBar>("myProgressBar");
            double currentValue = progressBar.Progress;
            currentValue += 0.1;
            progressBar.Progress = currentValue;
        }
        
        private void OnOrientationSensorReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            if (e.Reading != null)
            {
                // Obtenez l'inclinaison du téléphone en degrés
                double inclinationDegrees = GetInclinationDegrees(e.Reading.Orientation);

                // Mettez à jour l'étiquette avec l'inclinaison actuelle
                Device.BeginInvokeOnMainThread(() =>
                {
                    InclinationLabel.Text = $"{inclinationDegrees:F2}°";
                });
            }
        }

        private double GetInclinationDegrees(Quaternion quaternion)
        {
            // Convertir le quaternion en matrice de rotation
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(quaternion);

            // Extraire l'angle d'inclinaison de la matrice de rotation
            double inclinationRadians = Math.Asin(rotationMatrix.M23);
            double inclinationDegrees = inclinationRadians * (180.0 / Math.PI);

            return inclinationDegrees;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Démarrer l'écoute de l'orientation lorsque la page est affichée
            OrientationSensor.Start(SensorSpeed.UI);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Arrêter l'écoute de l'orientation lorsque la page n'est plus affichée
            OrientationSensor.Stop();
        }
        
        
    }
}