using DataAccess.Services;
using Microsoft.Maui.ApplicationModel.Communication;
using PharmaStoreInventory.Views;
using System.Net;
using System.Net.Mail;
using System.Windows.Input;

namespace PharmaStoreInventory;

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
                    await Navigation.PushAsync(page,true);
                }
            });

        BindingContext = this;

        //refrece.Text = $"{Helpers.AppPreferences.HostUserId},{Helpers.AppPreferences.HasBranchRegistered.ToString()}";

    }

    private void GoToStockDetailsView(object sender, EventArgs e)
    {
        Navigation.PushAsync(new StockDetailsView());
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        try
        {
            var ms = new MailingService();
            var code = await ms.SendVerificationCodeAsync("mofawzyhelal@gmail.com", null, "mohamed fawzy");

            //string fromMail = "modern.soft.2020@gmail.com";
            //string password = "mbzi wkby cqil vgrv ";

            //var message = new MailMessage();
            //message.From = new MailAddress(fromMail);
            //message.Subject = "test subject";
            //message.To.Add(new MailAddress("mofawzyhelal@gmail.com"));
            //message.Body = "<html><body> test body </body></html> ";
            //message.IsBodyHtml = true;

            //var smptClient = new SmtpClient("smtp.gmail.com")
            //{
            //    Port = 587,
            //    Credentials = new NetworkCredential(fromMail, password),
            //    EnableSsl = true,
            //};
            //smptClient.Send(message);

            //await Helpers.Alerts.DisplaySnackbar(code, 7);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

    }
}
