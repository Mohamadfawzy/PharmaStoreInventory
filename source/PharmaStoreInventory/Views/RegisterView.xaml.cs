using Microsoft.Maui.Controls;
using PharmaStoreInventory.Views.Templates;
using static System.Net.Mime.MediaTypeNames;

namespace PharmaStoreInventory.Views;

public partial class RegisterView : ContentPage
{
    

    public RegisterView()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        SetEntriesUnFocused();
        return true;
    }
    private async void Button_Clicked2(object sender, EventArgs e)
    {
        //await label.TranslateTo(-Width, 0, easing: Easing.SinInOut);


        //label.TranslationX = Width;
        //OnboardingText2.IsVisible = true;
        //new Animation
        //{
        //    { 0, 1.0, new Animation (v => OnboardingText1.TranslationX = v,0, -Width) },
        //    { 0, 1.0, new Animation (v => OnboardingText2.TranslationX = v, Width, 0)}
        //}.Commit(this, "ChildAnimations", 16, 500, easing: Easing.SinInOut);


        //var task1 = label.ScaleTo(0.8, length, Easing.SinInOut);
        //var task2 = label.TranslateTo(translationX, Ypoint, length, Easing.SinInOut);
        //await Task.WhenAll(task1, task2);


        //var task1 = label.ScaleTo(1, length, Easing.SinInOut);
        //var task2 = label.TranslateTo(0, 0, length, Easing.SinInOut);
        //await Task.WhenAll(task1, task2);

    }

    static VisualElement previousOwner;

    private void Button_Clicked(object sender, EventArgs e)
    {
        fullName.HideKeyBoard();
    }

    private void SetEntriesUnFocused()
    {
        foreach(var item in InputsContainer)
        {
            var entry = (item as AnimatedInput);
            if (entry != null)
            if (entry.EntryIsFocused())
            {
                entry.HideKeyBoard();
            };
        }
    }
}