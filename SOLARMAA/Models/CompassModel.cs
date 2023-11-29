using System.ComponentModel;

namespace SOLARMAA.Models;

// Modèle représentant le compas avec son angle et son point cardinal.
public class CompasModel : INotifyPropertyChanged
{
    // Propriétés
    private double _angle;

    private string _angleText;

    // Constructeur
    public CompasModel(double angle)
    {
        Angle = angle;
    }


    // Propriétés avec notification de changement de valeur
    public double Angle
    {
        get => _angle;
        set
        {
            _angle = value;
            AngleText = AngleTextCalcul(value);
            OnPropertyChanged(nameof(Angle));
        }
    }

    public string AngleText
    {
        get => _angleText;
        set
        {
            _angleText = value;
            OnPropertyChanged(nameof(AngleText));
        }
    }

    // Événement pour notifier le changement de propriété à la vue
    public event PropertyChangedEventHandler PropertyChanged;


    // Méthodes pour calculer le point cardinal en fonction de l'angle
    private static string AngleTextCalcul(double angle)
    {
        return angle switch
        {
            (>= 0 and < 22.5) or (>= 337.5 and <= 360) => "Nord", // Nord
            >= 22.5 and < 67.5 => "Nord-Est", // Nord-Est
            >= 67.5 and < 112.5 => "Est", // Est
            >= 112.5 and < 157.5 => "Sud-Est", // Sud-Est
            >= 157.5 and < 202.5 => "Sud", // Sud
            >= 202.5 and < 247.5 => "Sud-Ouest", // Sud-Ouest
            >= 247.5 and < 292.5 => "Ouest", // Ouest
            _ => "Nord-Ouest" // Nord-Ouest
        };
    }

    // Méthode pour notifier le changement de propriété à la vue
    private void OnPropertyChanged(string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}