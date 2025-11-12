using PharmaStoreInventory.Helpers;
using System.Windows.Input;

namespace PharmaStoreInventory.Views.Templates.V2;

public partial class StepperField : ContentView
{
    const string defaultText = "0";

    public static readonly BindableProperty TextProperty =
    BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(StepperField),
        defaultText, BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TitleProperty =
    BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(StepperField),
        "Title", BindingMode.TwoWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }


    public static readonly BindableProperty DescriptionProperty =
    BindableProperty.Create(
        nameof(Description),
        typeof(string),
        typeof(StepperField),
        "", BindingMode.TwoWay);

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public ICommand LongPressCommand => new Command(() => { Text = $"{defaultText}"; });
    public StepperField()
    {
        InitializeComponent();
    }

    private async void Plus_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = defaultText;
                return;
            }

            if (!int.TryParse(Text, out var number))
            {
                Text = defaultText;
                return;
            }

            // Increase the number by 1
            number++;

            Text = number.ToString();
            //await Alerts.DisplaySnackBar(Text);
        }
        catch (Exception ex)
        {
            Text = defaultText;
            await Alerts.DisplaySnackBar($"Error: {ex.Message}");
        }
    }

    private async void Minus_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = defaultText;
                return;
            }

            if (!int.TryParse(Text, out var number))
            {
                Text = defaultText;
                return;
            }

            // Decrease the number by 1 but not less than 0
            number = Math.Max(0, number - 1);

            Text = number.ToString();
           // await Alerts.DisplaySnackBar(Text);
        }
        catch (Exception ex)
        {
            Text = defaultText;
            await Alerts.DisplaySnackBar($"Error: {ex.Message}");
        }
    }


    public async Task HideSoftInputAsync()
    {
        await entry.HideSoftInputAsync(CancellationToken.None);
    }


    //private void Minus_Clicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (string.IsNullOrEmpty(Text))
    //        {
    //            Text = "0";
    //            return;
    //        }
    //        var number = decimal.Parse(Text);
    //        if (number <= 0)
    //            return;
    //        number--;
    //        Text = number.ToString();
    //    }
    //    catch 
    //    {
    //        Text = "0";
    //    }
    //}
}