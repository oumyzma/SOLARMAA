using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLARMAA.Services
{
    public class Gps
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public async Task<String> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                return "Erreur";
            }
            finally
            {
                _isCheckingLocation = false;
            }
            return "Erreur2";
        }


        public async Task<string> GetCurrentCity()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                {
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;

                    // Utilise l'API d'OSM pour obtenir le nom de la ville
                    string cityName = await GetCityNameFromOSMAsync(latitude, longitude);

                    return $"La ville est : {cityName}";
                }
            }
            catch (Exception ex)
            {
                // Unable to get location or city name
                return "Erreur lors de la récupération de la ville";
            }

            return "Erreur lors de la récupération de la ville";
        }

        private async Task<string> GetCityNameFromOSMAsync(double latitude, double longitude)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Utilise l'API de géocodage inversé d'OSM
                    string apiUrl = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                        string cityName = data.address.city;

                        return cityName;
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestion des erreurs de l'API OSM
                return "Erreur lors de la récupération du nom de la ville depuis OSM";
            }

            return "Erreur lors de la récupération du nom de la ville depuis OSM";
        }


        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
    }
}
