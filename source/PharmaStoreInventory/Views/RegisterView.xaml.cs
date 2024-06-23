using PharmaStoreInventory.Extensions;
using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Views;

public partial class RegisterView : ContentPage
{
    public RegisterView()
    {
        InitializeComponent();
    }
    //short count=0;
    //protected override bool OnBackButtonPressed()
    //{
    //    if (count == 0)
    //    {
    //        count = 1;
    //        inputsContainer.ClearFocusFromAllInputs();
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        inputsContainer.ClearFocusFromAllInputs();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if(ThisPage.Height > 100)
        {
            this.HeightRequest = ThisPage.Height + 400;

        }
    }
    //private void ClearFocusFromAllInputs()
    //{
    //    foreach (var item in InputsContainer)
    //    {
    //        var entry = (item as AnimatedInput);
    //        if (entry != null)
    //            if (entry.EntryIsFocused())
    //            {
    //                entry.HideKeyBoard();
    //            };
    //    }
    //}

}