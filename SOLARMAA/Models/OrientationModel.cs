
using System.ComponentModel;

namespace SOLARMAA.Models;

public class OrientationModel : INotifyPropertyChanged
{
    // Propriétés
    private double _pitch;
    private double _roll;
    private double _yaw;

    // Constructeur
    public OrientationModel(double x, double y, double z, double w)
    {
        Roll = x;
        Yaw = y;
        Pitch = z;
    }

    // Propriétés avec notification de changement de valeur
    public double Roll
    {
        get => _roll;
        set
        {
            _roll = value;
            OnPropertyChanged(nameof(Roll));
        }
    }

    public double Yaw
    {
        get => _yaw;
        set
        {
            _yaw = value;
            OnPropertyChanged(nameof(Yaw));
        }
    }

    public double Pitch
    {
        get => _pitch;
        set
        {
            _pitch = value;
            OnPropertyChanged(nameof(Pitch));
        }
    }

    // Événement pour notifier le changement de propriété à la vue
    public event PropertyChangedEventHandler PropertyChanged;

    // Méthode pour notifier le changement de propriété à la vue
    private void OnPropertyChanged(string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}