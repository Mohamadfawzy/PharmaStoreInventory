namespace PharmaStoreInventory.Triggers;

public class CloseBottomSheetTrigger : TriggerAction<VisualElement>
{
    protected async override void Invoke(VisualElement sender)
    {
        await sender.TranslateTo(0, 800, length: 250, easing: Easing.CubicInOut);
        sender.IsVisible = false;
    }
}