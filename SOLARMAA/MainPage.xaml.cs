using System;
using System.Numerics;
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
            // D�marrer l'�coute de l'orientation lorsque la page est affich�e
            OrientationSensor.Start(SensorSpeed.UI);
            Gps _gps = new Gps();
            /*var a = await _gps.GetCurrentLocation();
            await DisplayAlert("alert", a, "OK");*/
            var a = await _gps.GetCurrentCity();
            Device.BeginInvokeOnMainThread(() =>
            {
                Ville.Text = a;
            });


        }
        private void OnOrientationSensorReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            if (e.Reading != null)
            {
                // Obtenez l'inclinaison du t�l�phone en degr�s
                double inclinationDegrees = GetInclinationDegrees(e.Reading.Orientation);

                // Mettez � jour l'�tiquette avec l'inclinaison actuelle
                Device.BeginInvokeOnMainThread(() =>
                {
                    InclinationLabel.Text = $"{inclinationDegrees:F2}�";
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

            // Arr�ter l'�coute de l'orientation lorsque la page n'est plus affich�e
            OrientationSensor.Stop();
        }
    }
}