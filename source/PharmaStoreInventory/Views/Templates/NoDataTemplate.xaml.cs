using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;
using PharmaStoreInventory.Models;

namespace PharmaStoreInventory.Views.Templates;

public partial class NoDataTemplate : ContentView
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(NoDataTemplate));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public static readonly BindableProperty Command2Property =
        BindableProperty.Create(
        nameof(Command2),
        typeof(ICommand),
        typeof(NoDataTemplate));

    public ICommand Command2
    {
        get => (ICommand)GetValue(Command2Property);
        set => SetValue(Command2Property, value);
    }


    public static readonly BindableProperty DataProperty =
        BindableProperty.Create(
        nameof(Data),
        typeof(NoDataModel),
        typeof(NoDataTemplate));

    public NoDataModel Data
    {
        get => (NoDataModel)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly BindableProperty stringProperty =
        BindableProperty.Create(
        nameof(ButtonText),
        typeof(string),
        typeof(NoDataTemplate),
        "Try again");

    public string ButtonText
    {
        get => (string)GetValue(stringProperty);
        set => SetValue(stringProperty, value);
    }

    public static readonly BindableProperty ButtonText2Property =
       BindableProperty.Create(
       nameof(ButtonText2),
       typeof(string),
       typeof(NoDataTemplate),
       " ");

    public string ButtonText2
    {
        get => (string)GetValue(ButtonText2Property);
        set => SetValue(ButtonText2Property, value);
    }

    public NoDataTemplate()
    {
        InitializeComponent();
    }


}