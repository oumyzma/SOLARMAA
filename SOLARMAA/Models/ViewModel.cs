using System.ComponentModel;

namespace SOLARMAA.Models;

public class ViewModel : INotifyPropertyChanged
{
    // Propriétés
    private CompasModel _compasModel;


    // Constructeur
    public ViewModel(CompasModel compasModel)
    {
        CompasModel = compasModel;
    }

    // Propriétés avec notification de changement de valeur
    public CompasModel CompasModel
    {
        get => _compasModel;
        set
        {
            _compasModel = value;
            OnPropertyChanged(nameof(CompasModel));
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