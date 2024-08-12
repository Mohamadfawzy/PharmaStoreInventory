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

    public StepperField()
	{
		InitializeComponent();
	}

    private void Plus_Clicked(object sender, EventArgs e)
    {
        var number = decimal.Parse(Text);
        number++;
        Text = number.ToString();
    }
    
    private void Minus_Clicked(object sender, EventArgs e)
    {
        var number = decimal.Parse(Text);
        number--;
        Text = number.ToString();

    }
}