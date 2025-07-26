using ApiSettingsManager.Models;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ApiSettingsManager;
public partial class UpdateAPIWindow : Window
{
    private bool status = false;
    private string statusRunningString = "Status: Running", statusCloseString = "Status: Stopped";
    private string FullPathApiFolder = AppDomain.CurrentDomain.BaseDirectory;
    private readonly string ApiExeFileName = "PharmaInventoryAPI";
    public UpdateAPIWindow()
    {
        InitializeComponent();
        Init();
        textBoxThisPcIp.Text = GetLocalIPAddress();
    }
    void Init()
    {
#if DEBUG
        FullPathApiFolder = @"D:\MoSoft_Projects\Backend\Publish";
#endif
        LoadSettings();
        CheckStatus();
    }

    public void LoadSettings()
    {
        try
        {
            string appSettingsPath = Path.Combine(FullPathApiFolder, "appsettings.json");

            if (File.Exists(appSettingsPath))
            {
                string jsonString = File.ReadAllText(appSettingsPath);

                var jsonNode = JsonNode.Parse(jsonString);

                var url = jsonNode?["ConnectionStrings"]?["applicationUrl"]?.ToString();
                var server = jsonNode?["ConnectionStrings"]?["server"]?.ToString();

                if (url != null)
                {
                    var appUrl = new Uri(url);
                    apiIpTextBox.Text = appUrl.Host;
                    apiPortTextBox.Text = appUrl.Port.ToString();
                }

                if (server != null)
                {
                    var dbServerDetails = server.Trim(';').Split(',');
                    if (dbServerDetails.Length == 2)
                    {
                        databaseIpTextBox.Text = DecryptString(dbServerDetails[0], "bRyCqldrbgML3LXw", "y6OTIsYu8JE1wLkX");
                        databasePortTextBox.Text = dbServerDetails[1];
                    }
                }
            }
        }
        catch
        {
        }
    }
    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            await KillProcesses();
            // الحصول على المدخلات من المستخدم
            string thisPcIP = apiIpTextBox.Text;
            string apiPort = apiPortTextBox.Text;
            string databaseServer = databaseIpTextBox.Text;
            string databasePort = databasePortTextBox.Text;

            // تحديد مسار ملف appsettings.json
            string appSettingsPath = Path.Combine(FullPathApiFolder, "appsettings.json");

            if (File.Exists(appSettingsPath))
            {
                // قراءة محتوى appsettings.json
                string jsonString = File.ReadAllText(appSettingsPath);

                // Parse the JSON string into a JsonNode (mutable)
                var jsonNode = JsonNode.Parse(jsonString);
                var dbipEnc = EncryptString(databaseServer, "bRyCqldrbgML3LXw", "y6OTIsYu8JE1wLkX");
                // Modify applicationUrl و connection string
                if (jsonNode != null)
                {
                    jsonNode["ConnectionStrings"]["applicationUrl"] = $"http://{thisPcIP}:{apiPort}";
                    jsonNode["ConnectionStrings"]["server"] = $"{dbipEnc},{databasePort};";
                }


                // Serialize the JsonNode back to a JSON string
                File.WriteAllText(appSettingsPath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                MessageBox.Show("Settings saved successfully!");
            }
            else
            {
                MessageBox.Show("appsettings.json not found!");
            }

        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
        finally
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            CheckStatus();
        }

    }

    private void RunApiButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            // run MyApi.exe
            string apiPath = Path.Combine(FullPathApiFolder, ApiExeFileName + ".exe");
            if (File.Exists(apiPath))
            {
                Process.Start(apiPath);
                SetStatus();
            }
            else
            {
                MessageBox.Show("the file exe not found!");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while running file.exe: {ex.Message}");
        }
        finally
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
    }

    private async void TestApiClick(object sender, RoutedEventArgs e)
    {
        await TestConnectionAsync(apiIpTextBox.Text, apiPortTextBox.Text);
    }
    private async Task<bool> TestConnectionAsync(string ip, string port)
    {
        var url = $"http://{ip}:{port}/api/Employee/test_connection";
        try
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Result<string>?>().ConfigureAwait(false);

            if (result == null)
            {
                MessageBox.Show($"حدث خطأ تحقق من البيانات");
                return false;
            }

            if (result.IsSuccess)
            {
                MessageBox.Show($"Test connection succeeded.");
                return true;
            }
            else
            {
                MessageBox.Show(result.Message);
                return false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);

            return false;
        }
        finally
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
    }
    private async void StopApiButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            // العثور على العملية التي تعمل باسم MyApi.exe
            var processes = Process.GetProcessesByName(ApiExeFileName);

            if (await KillProcesses())
            {
                SetStatus(false);
                MessageBox.Show("MyAPI has been stopped.");
            }
            else
            {
                MessageBox.Show("MyAPI is not running.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error while stopping {ApiExeFileName}: {ex.Message}");
        }
        finally
        {
            CheckStatus();
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }
    }

    Task<bool> KillProcesses()
    {
        var processes = Process.GetProcessesByName(ApiExeFileName);

        if (processes.Length > 0)
        {
            foreach (var process in processes)
            {
                process.Kill();
                return Task.FromResult(true);
            }
        }
        return Task.FromResult(false);
    }


    static string GetLocalIPAddressOld()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                // نفترض أنك تريد عنوان IPv4 فقط
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }
        catch (Exception)
        {

            MessageBox.Show("لم يتم العثور علي ip");
            return "";
        }
    }


    static string GetLocalIPv4Address()
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            // تأكد من أن الكارت مفعل ويعمل
            if (ni.OperationalStatus == OperationalStatus.Up &&
                (ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                 ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
            {
                var ipProps = ni.GetIPProperties();

                foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.Address.ToString();
                    }
                }
            }
        }

        MessageBox.Show("لم يتم العثور على عنوان IPv4 صالح.");
        return "";
    }

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        catch
        {

        }
    }

    private void Image_MouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Application.Current.Shutdown();
        }
        catch
        {
        }
    }

    private async void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Clipboard.SetText(textBoxThisPcIp.Text);
            textBlockCopy.FontSize = 10;
            await Task.Delay(1000);
            textBlockCopy.FontSize = 14;

        }
        catch
        {
        }
    }

    private void CheckStatus()
    {

        //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        var processes = Process.GetProcessesByName(ApiExeFileName);

        if (processes.Length > 0)
        {
             SetStatus();
        }
        else
        {
            SetStatus(false);
        }
    }

    void SetStatus(bool status = true)
    {
        if (status)
        {
            statusBorder.Background = new SolidColorBrush(Color.FromRgb(40, 167, 69));
            statusTextBlock.Text = statusRunningString;
            runApiButton.IsEnabled = false;
            testApiButton.IsEnabled = true;
            closeApiButton.IsEnabled = true;

        }
        else
        {
            statusBorder.Background = new SolidColorBrush(Color.FromRgb(220, 53, 69));
            statusTextBlock.Text = statusCloseString;
            runApiButton.IsEnabled = true;
            testApiButton.IsEnabled = false;
            closeApiButton.IsEnabled = false;
        }
    }
    public static string EncryptString(string plainText, string key, string iv)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cs))
        {
            writer.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    public static string DecryptString(string cipherText, string key, string iv)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        byte[] buffer = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.IV = ivBytes;

        using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
        using (var ms = new MemoryStream(buffer))
        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        using (var reader = new StreamReader(cs))
        {
            return reader.ReadToEnd();
        }
    }
}
