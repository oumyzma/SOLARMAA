using System;
using System.Numerics;
using Android.Telephony;
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
            PourcentageHome.Text = $"{inclinationDegrees/90*100:F0}";
            await ProgressBarHome.ProgressTo(inclinationDegrees / 90, 300, Easing.Linear);
            if (inclinationDegrees > 80)
            {
                ProgressBarHome.ProgressColor = Colors.Cyan;
                TexteRendement.Text = "Votre emplacement est parfait !";
                TexteRendement.TextColor = Colors.Cyan;
            }
            else if (inclinationDegrees > 70)
            {
                ProgressBarHome.ProgressColor = Colors.LightGreen;
                TexteRendement.Text = "Votre emplacement est presque parfait !";
                TexteRendement.TextColor = Colors.LightGreen;

            }
            else if (inclinationDegrees > 60)
            {
                ProgressBarHome.ProgressColor = Colors.YellowGreen;
                TexteRendement.Text = "Votre emplacement est bien !";
                TexteRendement.TextColor = Colors.YellowGreen;

            }
            else if (inclinationDegrees > 45)
            {
                ProgressBarHome.ProgressColor = Colors.Yellow;
                TexteRendement.Text = "Votre emplacement est pas super";
                TexteRendement.TextColor = Colors.Yellow;

            }
            else if (inclinationDegrees > 30)
            {
                ProgressBarHome.ProgressColor = Colors.Orange;
                TexteRendement.Text = "Votre emplacement est pas rentable";

                TexteRendement.TextColor = Colors.Orange;
            }
            else if (inclinationDegrees > 15)
            {
                ProgressBarHome.ProgressColor = Colors.OrangeRed;
                TexteRendement.Text = "Votre emplacement est nul";
                TexteRendement.TextColor = Colors.OrangeRed;

            }
            else
            {
                ProgressBarHome.ProgressColor = Colors.Red;
                TexteRendement.Text = "Votre emplacement est super nul";
                TexteRendement.TextColor = Colors.Red;

            }

        }
    }
}