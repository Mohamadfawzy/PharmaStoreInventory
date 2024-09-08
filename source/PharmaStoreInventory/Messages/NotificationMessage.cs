using CommunityToolkit.Mvvm.Messaging.Messages;
using PharmaStoreInventory.Models;
namespace PharmaStoreInventory.Messages;

public class NotificationMessage : ValueChangedMessage<ErrorMessage>
{
    public NotificationMessage(ErrorMessage value) : base(value)
    {
        
    }
}