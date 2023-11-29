
using System.Numerics;
using SOLARMAA.Models;
using SOLARMAA.Services;

namespace SOLARMAA

{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private static CompasModel _compasModel = new(0);
        

        private readonly ISensor _sensor = MauiApplication.Current.Services.GetService<ISensor>();
        private readonly ViewModel _viewModel = new(_compasModel);
        
        public MainPage()
        {
            InitializeComponent();

            OrientationSensor.ReadingChanged += OnOrientationSensorReadingChanged;
            
            // Vérifie si le GPS est supporté sur le téléphone et affiche une alerte si ce n'est pas le cas sinon démarre le GPS
            if (!_sensor.ToggleCompass()) DisplayAlert("Alert", "Compass not supported on device", "OK");
            // Vérifie si le compas est supporté sur le téléphone et affiche une alerte si ce n'est pas le cas sinon démarre le compas
            
            // Définit le BindingContext de la page sur le ViewModel
            BindingContext = _viewModel;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Démarrer l'écoute de l'orientation lorsque la page est affichée
            OrientationSensor.Start(SensorSpeed.UI);
          
            Gps _gps = new Gps();
            var a = await _gps.GetCurrentLocation();
            await DisplayAlert("alert", a, "OK");
            if (_compasModel != null)
                // Met à jour le modèle avec l'angle du compas
                _compasModel = _sensor.CompassText;
          
            
            _viewModel.CompasModel = _compasModel;

        }
        private void OnOrientationSensorReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            if (e.Reading != null)
            {
                // Obtain the phone's tilt in degrees
                double inclinationDegrees = GetInclinationDegrees(e.Reading.Orientation);
          

                // Update the label with the current inclination
                MainThread.BeginInvokeOnMainThread(() =>
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Arrêter l'écoute de l'orientation lorsque la page n'est plus affichée
            OrientationSensor.Stop();
        }
        
    }
}