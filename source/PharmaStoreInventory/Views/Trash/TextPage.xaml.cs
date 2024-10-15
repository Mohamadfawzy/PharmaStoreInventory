using PharmaStoreInventory.Views.Templates;
using System.ComponentModel;

namespace PharmaStoreInventory.Views.Trash;

public partial class TextPage : ContentPage
{
    public TextPage()
    {
        InitializeComponent();
    }
    AnimatedInput Input1;
    AnimatedInput Input2;
    AnimatedInput Input3;
    AnimatedInput Input4;
    AnimatedInput Input5;
    AnimatedInput Input6;
    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        Input1 = new()
        {
            InputText = "text 1",
            InputPlaceholder = "place 1",
            ErrorMessage = "ErrorMessage"
        };
        AddToContainer(Input1);
        await Task.Delay(1);
        
        Input2 = new()
        {
            InputText = "text 1",
            InputPlaceholder = "place 1",
            ErrorMessage = "ErrorMessage"
        };
        AddToContainer(Input2);
        await Task.Delay(1);

        Input3 = new()
        {
            InputText = "text 1",
            InputPlaceholder = "place 1",
            ErrorMessage = "ErrorMessage"
        };
        AddToContainer(Input3);
        await Task.Delay(1);
        
        Input4 = new()
        {
            InputText = "text 1",
            InputPlaceholder = "place 1",
            ErrorMessage = "ErrorMessage"
        };
        AddToContainer(Input4);
        await Task.Delay(1);

        Input5 = new()
        {
            InputText = "text 1",
            InputPlaceholder = "place 1",
            ErrorMessage = "ErrorMessage"
        };
        AddToContainer(Input5);
        await Task.Delay(1);

        Input6 = new()
        {
            InputText = "text 1",
            InputPlaceholder = "place 1",
            ErrorMessage = "ErrorMessage"
        };
        AddToContainer(Input6);
    }

    void AddToContainer(AnimatedInput input)
    {
      
            container.Add(input);
    }
}