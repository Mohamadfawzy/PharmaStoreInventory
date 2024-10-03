using System.Windows.Input;

namespace PharmaStoreInventory.Views.Templates;

public partial class StepperField : ContentView
{
    public static readonly BindableProperty TextProperty =
    BindableProperty.Create(
    nameof(Text),
    typeof(string),
    typeof(StepperField),
    "0", BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public ICommand LongPressCommand => new Command(() => { Text = $"{0}"; });
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
                Text = "0";
                return;
            }
            var number = decimal.Parse(Text);
            number++;
            Text = number.ToString();
        }
        catch
        {
            Text = "0";
        }
    }
    
    private void Minus_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = "0";
                return;
            }
            var number = decimal.Parse(Text);
            if (number <= 0)
                return;
            number--;
            Text = number.ToString();
        }
        catch 
        {
            Text = "0";
        }
    }
}