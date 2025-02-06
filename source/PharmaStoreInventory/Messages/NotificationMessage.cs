using CommunityToolkit.Mvvm.Messaging.Messages;
using PharmaStoreInventory.Models;
namespace PharmaStoreInventory.Messages;

public class NotificationMessage(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}

public class DashboardViewNotification(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}

public class CreateBranchViewNotification(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}

public class PickingViewNotification(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}

public class RegisterViewNotification(ErrorMessage value) : ValueChangedMessage<ErrorMessage>(value)
{
}

public class StepperFieldNotification(int value) : ValueChangedMessage<int>(value)
{
}