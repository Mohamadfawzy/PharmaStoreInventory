using CommunityToolkit.Mvvm.Messaging;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Messages;
using System.Globalization;
using System.Windows.Input;

namespace PharmaStoreInventory.Views.Templates;

public partial class StepperField : ContentView
{
    const string defaultText = "0.00";
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
    public ICommand LongPressCommand => new Command(() => { Text = $"{defaultText}"; });
    public StepperField()
    {
        InitializeComponent();
    }

    private void Plus_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = defaultText;
                return;
            }

            // Parse using InvariantCulture to support decimal numbers correctly
            if (double.TryParse(Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                number++;
                Text = number.ToString("F2", CultureInfo.InvariantCulture);
            }
            else
            {
                Text = defaultText;
            }
        }
        catch (Exception ex)
        {
            Text = defaultText;
            Alerts.DisplaySnackBar(ex.Message);
        }
    }

    private void Minus_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = defaultText;
                return;
            }

            // Parse using InvariantCulture to support decimal numbers correctly
            if (double.TryParse(Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                if (number <= 1 && number >= 0)
                {
                    number -= 0.50;
                }
                else
                    number--;

                // Prevent going below zero (if needed)
                if (number < 0)
                    number = 0;

                Text = number.ToString("F2", CultureInfo.InvariantCulture);
            }
            else
            {
                Text = defaultText;
            }
        }
        catch (Exception ex)
        {
            Text = defaultText;
            Alerts.DisplaySnackBar(ex.Message);
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