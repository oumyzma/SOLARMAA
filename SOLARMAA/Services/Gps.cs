using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLARMAA.Services
{
    public class Gps
    {
        public async Task<string> GetCachedLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return "None";
        }

        public async Task<string> GetCityFromCoordinates(double latitude, double longitude)
        {
            try
            {
                // Utiliser le service de géocodage inversé pour obtenir le nom de la ville
                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var city = placemarks?.FirstOrDefault()?.Locality;

                if (!string.IsNullOrEmpty(city))
                    return city;
                else
                    return "Nom de la ville non disponible";
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Gérer l'exception de fonctionnalité non prise en charge sur l'appareil
            }
            catch (Exception ex)
            {
                // Gérer l'exception générale
            }

            return "Impossible de déterminer la localisation";
        }


    }
}
