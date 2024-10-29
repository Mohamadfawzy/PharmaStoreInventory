using System.Diagnostics;
using System.Windows.Input;

namespace AdminApp;

public partial class MainPage : ContentPage
{
    public ICommand NavigateCommand { get; private set; }

    public MainPage()
    {
        InitializeComponent();

        NavigateCommand = new Command<Type?>(
            async (Type? pageType) =>
            {
                if (pageType != null)
                {
                    Page? page = Activator.CreateInstance(pageType) as Page;
                    await Navigation.PushAsync(page, true);
                }
            });

        BindingContext = this;
    }
    string x = "===============================================================";
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Console.WriteLine($"{Environment.NewLine}StartAt:{ stopWatch.Elapsed.Microseconds.ToString()}");

        await Navigation.PushAsync(new Views.LoginView());

        stopWatch.Stop();
        Console.WriteLine(stopWatch.Elapsed.Microseconds.ToString());
        Console.WriteLine($"\n{x}\n{x}\n{x}\n{x}\n{x}\n{x}\n{x}\n{x}\n{x}\n{x}\n{x}");
    }
}
