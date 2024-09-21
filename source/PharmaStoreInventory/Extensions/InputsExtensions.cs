using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Extensions;
public static class InputsExtensions
{
    public static void ClearFocusFromAllInputs(this VerticalStackLayout inputsContainer)
    {
        foreach (var item in inputsContainer.Children)
        {
            var input = item as AnimatedInput;
            if (input != null && input.EntryIsFocused())
            {
                input.HideKeyBoard();
            }
        }
    }
    
    public static async Task PositioningOfPlaceHolder(this VerticalStackLayout inputsContainer)
    {
        foreach (var item in inputsContainer.Children)
        {
            await Task.Delay(300);
            var input = item as AnimatedInput;
            input?.PositioningOfPlaceHolder();
        }
    }
}
