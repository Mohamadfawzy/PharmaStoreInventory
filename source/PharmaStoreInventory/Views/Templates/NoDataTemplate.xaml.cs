using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;

namespace PharmaStoreInventory.Views.Templates;

public partial class NoDataTemplate : ContentView
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(SearchBoxTemplate));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    public NoDataTemplate()
	{
		InitializeComponent();
	}
}