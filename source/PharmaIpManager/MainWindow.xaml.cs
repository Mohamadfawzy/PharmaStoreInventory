using ApiSettingsManager.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApiSettingsManager;

public partial class MainWindow : Window
{
    private string BaseUrl = "http://usersapi.modern-soft.com:5252/api/";
    public MainWindow()
    {
        InitializeComponent();
        Init();
    }
    void Init()
    {
#if DEBUG
        BaseUrl = " http://192.168.1.103:5219/api/";
#endif
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            // Simulate button click when Enter is pressed
            signinButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
    private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
    {
        txtEmail.Focus();
    }

    private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
        {
            textEmail.Visibility = Visibility.Collapsed;
        }
        else
        {
            textEmail.Visibility = Visibility.Visible;
        }
    }

    private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
    {
        txtPassword.Focus();
    }

    private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
        {
            textPassword.Visibility = Visibility.Collapsed;
        }
        else
        {
            textPassword.Visibility = Visibility.Visible;
        }
    }

    private async void Signin_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            signinButton.IsEnabled = false;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            if (string.IsNullOrEmpty(txtEmail.Text) && string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show($"please write valid input");
                return;
            }

            var login = new LoginRequest(txtEmail.Text, txtPassword.Password);
            var status = await LoginAsync(login);
            if (!status)
                return;

            // Create a new instance of the NewWindow
            UpdateAPIWindow newWindow = new UpdateAPIWindow();

            // Show the new window
            newWindow.Show();

            // Close the current window
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Exception {ex.Message}");
        }
        finally
        {
            signinButton.IsEnabled = true;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            this.DragMove();
        }
    }

    private void Image_MouseUp(object sender, MouseButtonEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async Task<bool> LoginAsync(LoginRequest data)
    {
        try
        {
            var FullUrl = BaseUrl + "admin/login";

            using var client = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, FullUrl)
            {
                Content = JsonContent.Create(data)
            };

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Result<UserLoginResponse>?>().ConfigureAwait(false);

            if (result == null)
            {
                return false;
            }

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                MessageBox.Show(result.Message);
            }
            return false;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return false;
        }
    }

    private void Button_Click2(object sender, RoutedEventArgs e)
    {
        // Create a new instance of the NewWindow
        UpdateAPIWindow newWindow = new UpdateAPIWindow()
        {
            Title = "Modal Dialog Window",
            Topmost = true,
            ResizeMode = ResizeMode.NoResize,
            ShowInTaskbar = false,
        };

        // Show the new window
        newWindow.Show();

        // Close the current window
        this.Close();
    }
}
