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
}
