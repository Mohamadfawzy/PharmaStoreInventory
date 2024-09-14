using CommunityToolkit.Mvvm.Messaging.Messages;
using PharmaStoreInventory.Models;
namespace PharmaStoreInventory.Messages;

public class NotificationMessage(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}

public class DashboardViewNotification(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}

public class CreateBranchViewotification(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}