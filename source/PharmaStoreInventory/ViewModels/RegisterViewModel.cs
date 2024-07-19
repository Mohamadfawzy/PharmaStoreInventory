using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
namespace PharmaStoreInventory.ViewModels;

public class RegisterViewModel : ObservableObject
{
    private string name = string.Empty;
    private string invalidMessage = string.Empty;
    private bool showError;

    public RegisterViewModel()
    {
        //name = "الاسم من الفيو موديل";
        Task.Delay(5000);
        Awaiting();
        
    }

    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    async void  Awaiting()
    {
        await Task.Delay(5000);
        Awaiting2();
    } 
    async void Awaiting2()
    {
        await Task.Delay(5000);
    }
    public string InvalidMessage
    {
        get => invalidMessage;
        set => SetProperty(ref invalidMessage, value);
    }

    public bool ShowError
    {
        get => showError;
        set => SetProperty(ref showError, value);
    }

    public ICommand GetNameCommand => new Command(GetNameEx);


    private void GetNameEx()
    {
        //Helpers.CatchingException.PharmaDisplayAlert(Name);
        //InvalidMessage = "رسالة من الفييو موديل";
        //ShowError = !ShowError;
        Name = "سسسسسسسسسسسسسسسسسسسسسسشسيبشسيبشسيبشس";
    }
}
