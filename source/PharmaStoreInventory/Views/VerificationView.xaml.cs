using PharmaStoreInventory.Views.Templates;

namespace PharmaStoreInventory.Views;

public partial class VerificationView : ContentPage
{
    public VerificationView()
    {
        InitializeComponent();
        StartResendCodeTimer();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private void ThisPage_Loaded(object sender, EventArgs e)
    {

        //for (int i = 0; i < 10; i++)
        //{
        //    var inputFaild = new AnimatedInput();
        //    inputFaild.InputPlaceholder = "ايمييل";
        //    container.Add(inputFaild);
        //}
    }

    private void ThisPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        entry1.Focus();
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            var rcp = entry.ReturnCommandParameter.ToString();
            if (rcp != null)
            {
                if (string.IsNullOrEmpty(entry.Text)) // Backspace
                {
                    switch (rcp)
                    {
                        case "4":
                            entry3.IsReadOnly = false;
                            entry3.Focus();
                            entry4.IsReadOnly = true;
                            break;

                        case "3":
                            entry2.IsReadOnly = false;
                            entry2.Focus();
                            entry3.IsReadOnly = true;
                            break;

                        case "2":
                            entry1.IsReadOnly = false;
                            entry1.Focus();
                            entry2.IsReadOnly = true;
                            break;

                        case "1":

                            break;

                    }

                    //var ent = entriesContainer.FindByName("entry"+rcp) as Entry;
                    //ent.IsEnabled = false;
                }
                else // Write
                {
                    switch (rcp)
                    {
                        case "1":
                            entry2.IsReadOnly = false;
                            entry2.Focus();
                            //checkLenth(ref entry1, entry2);
                            break;

                        case "2":
                            entry3.IsReadOnly = false;
                            entry3.Focus();
                            break;

                        case "3":
                            entry4.IsReadOnly = false;
                            entry4.Focus();
                            break;

                        case "4":
                            break;
                    }
                }
            }
        }
    }

    private void OnFocused(object sender, FocusEventArgs e)
    {
        //if (sender is Entry entry)
        //    entry.Text = string.Empty;
    }

    private async  void Button_Clicked(object sender, EventArgs e)
    {
        await Helpers.Alerts.DisplaySnackbar($"{entry1.Text}{entry2.Text}{entry3.Text}{entry4.Text}");
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void PopTapped(object sender, TappedEventArgs e)
    {
        Navigation.PopAsync();
    }

    private int _timerSeconds;
    private bool _isTimerRunning;

    private void OnResendCodeClicked(object sender, EventArgs e)
    {
        // Your logic to resend the verification code

        // Start the timer again
        StartResendCodeTimer();
    }

    private async void StartResendCodeTimer()
    {
        _timerSeconds = 60;
        _isTimerRunning = true;
        ResendCodeButton.IsEnabled = false;
        TimerLabel.IsVisible = true;
        TimerLabel.Text = $"You can resend the code in {_timerSeconds} seconds";

        while (_isTimerRunning && _timerSeconds > 0)
        {
            await Task.Delay(1000); // Wait for 1 second
            _timerSeconds--;
            TimerLabel.Text = $"You can resend the code in {_timerSeconds} seconds";
        }

        _isTimerRunning = false;
        TimerLabel.IsVisible = false;
        ResendCodeButton.IsEnabled = true;
    }

}