using System;
using System.Numerics;
using Android.Telephony;
using SOLARMAA.Services;
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
            // Démarrer l'écoute de l'orientation lorsque la page est affichée
            OrientationSensor.Start(SensorSpeed.UI);
            Gps _gps = new Gps();
            var a = await _gps.GetCurrentLocation();
            await DisplayAlert("alert", a, "OK");


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
            Affichage(inclinationDegrees);
            return inclinationDegrees;
        }
        
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Arrêter l'écoute de l'orientation lorsque la page n'est plus affichée
            OrientationSensor.Stop();
        }
        async private void Affichage(double inclinationDegrees)
        {
            PourcentageHome.Text = $"{32}°";
            await ProgressBarHome.ProgressTo(inclinationDegrees/100, 200, Easing.Linear);
            if (inclinationDegrees > 75)
            {
                ProgressBarHome.ProgressColor = Colors.Red;
            }
            else if (inclinationDegrees > 50)
            {
                ProgressBarHome.ProgressColor = Colors.Orange;
            }
            else if (inclinationDegrees > 25)
            {
                ProgressBarHome.ProgressColor = Colors.Yellow;
            }
            else
            {
                ProgressBarHome.ProgressColor = Colors.GreenYellow;
            }

        }
    }
}