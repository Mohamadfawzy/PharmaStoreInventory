using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace PharmaStoreInventory.ViewModels;

public class BaseViewModel : ObservableObject
{
    private bool activityIndicatorRunning = false;

    public bool ActivityIndicatorRunning
    {
        get => activityIndicatorRunning;
        set => SetProperty(ref activityIndicatorRunning, value);
    }
}
