using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using PharmaStoreInventory.Messages;
using PharmaStoreInventory.Models;
using PharmaStoreInventory.ViewModels;

namespace PharmaStoreInventory.Views;

public partial class EditProAmountView : ContentPage, IRecipient<NotificationMessage>
{
    
	public EditProAmountView(ProductUnitsDto product)
	{
		InitializeComponent();
        this.BindingContext = new EditProAmountViewModel(product);
        WeakReferenceMessenger.Default.Register<NotificationMessage>(this);
    }

    public void Receive(NotificationMessage message)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            notification.Display(message.Value);
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if ANDROID
        Microsoft.Maui.Controls.Application.Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                 .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
#endif
    }
    private async void OnCloseClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}