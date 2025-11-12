using System.Windows.Input;

namespace PharmaStoreInventory.Views.Templates.V2;

public partial class DateSpinner : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(UpDownStepper),
            "0", BindingMode.TwoWay);
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty TextPlaceholderProperty =
        BindableProperty.Create(
            nameof(TextPlaceholder),
            typeof(string),
            typeof(UpDownStepper),
            "أدخل قيمة", BindingMode.TwoWay);

    public string TextPlaceholder
    {
        get => (string)GetValue(TextPlaceholderProperty);
        set => SetValue(TextPlaceholderProperty, value);
    }


    public static readonly BindableProperty MinimumValueProperty =
        BindableProperty.Create(
            nameof(MinimumValue),
            typeof(int),
            typeof(UpDownStepper),
            1, BindingMode.TwoWay);

    public int MinimumValue
    {
        get => (int)GetValue(MinimumValueProperty);
        set => SetValue(MinimumValueProperty, value);
    }


    public static readonly BindableProperty MaximumValueProperty =
        BindableProperty.Create(
            nameof(MaximumValue),
            typeof(int),
            typeof(UpDownStepper),
            9999, BindingMode.TwoWay);

    public int MaximumValue
    {
        get => (int)GetValue(MaximumValueProperty);
        set => SetValue(MaximumValueProperty, value);
    }

    public ICommand UpLongPressCommand => new Command(() =>
    {
        Text = MaximumValue.ToString();
    });
    public ICommand DownLongPressCommand => new Command(() =>
    {
        Text = MinimumValue.ToString();
    });


    public DateSpinner()
    {
        InitializeComponent();
    }

    private void Up_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = MaximumValue.ToString();
                return;
            }
            var number = int.Parse(Text);
            if (number >= MaximumValue)
            {
                Text = MaximumValue.ToString();
                return;
            }
            number++;
            Text = number.ToString();
        }
        catch
        {
            Text = MaximumValue.ToString();
        }
    }

    private void Down_Clicked(object sender, EventArgs e)
    {
        try
        {

            if (string.IsNullOrWhiteSpace(Text))
            {
                Text = MinimumValue.ToString();
                return;
            }
            var number = int.Parse(Text);
            if (number <= MinimumValue)
            {
                Text = MinimumValue.ToString();
                return;
            }
            number--;
            Text = number.ToString();
        }
        catch
        {
            Text = MinimumValue.ToString();
        }
    }

    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Text))
            {
                Text = MinimumValue.ToString();
                return;
            }

            if (int.Parse(Text) <= MinimumValue)
            {
                Text = MinimumValue.ToString();
            }
            if (int.Parse(Text) >= MaximumValue)
            {
                Text = MaximumValue.ToString();
            }
        }
        catch
        {
            Text = MinimumValue.ToString();
        }
    }


    public void SetEntryUnfocus()
    {
        entry.Unfocus();
    }
}