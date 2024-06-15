using System.Windows.Input;

namespace PharmaStoreInventory.ViewModels;

public class RegisterViewModel
{
    public RegisterViewModel()
    {
            
    }

    public string Name { get; set; } 

    public ICommand GetNameCommand => new Command(GetNameEx);


    private void GetNameEx()
    {
        App.Current.MainPage.DisplayAlert("Title", Name, "cancel");
    }
}
