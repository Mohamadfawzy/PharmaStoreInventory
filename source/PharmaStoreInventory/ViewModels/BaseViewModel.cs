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
    
    private bool isRefreshing = false;

    public bool IsRefreshing
    {
        get => isRefreshing;
        set => SetProperty(ref isRefreshing, value);
    }
    
    private bool isEmptyColleciton = true;

    public bool IsEmptyColleciton
    {
        get => isEmptyColleciton;
        set => SetProperty(ref isEmptyColleciton, value);
    }
}
