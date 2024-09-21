using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

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

    private bool isPlaceholderVisible = true;

    public bool IsPlaceholderVisible
    {
        get => isPlaceholderVisible;
        set => SetProperty(ref isPlaceholderVisible, value);
    }

    private bool isNoDataElementVisible = false;
    public bool IsNoDataElementVisible
    {
        get => isNoDataElementVisible;
        set => SetProperty(ref isNoDataElementVisible, value);
    }

    private bool isEmptyViewVisible = true;
    public bool IsEmptyViewVisible
    {
        get => isEmptyViewVisible;
        set => SetProperty(ref isEmptyViewVisible, value);
    }

    public ICommand NavigationPopCommand => new Command(async () =>
    {
        if (Application.Current != null && Application.Current.MainPage != null)
            await Application.Current.MainPage.Navigation.PopAsync();
    });
}
