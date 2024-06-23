using System.Windows.Input;

namespace PharmaStoreInventory.Views.Templates;

public partial class SearchBoxTemplate : ContentView
{
    public SearchBoxTemplate()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(SearchBoxTemplate));

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(SearchBoxTemplate),
        string.Empty, BindingMode.TwoWay);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

}