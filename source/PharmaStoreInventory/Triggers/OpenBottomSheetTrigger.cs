namespace PharmaStoreInventory.Triggers;

public class OpenBottomSheetTrigger : TriggerAction<VisualElement>
{
    protected async override void Invoke(VisualElement sender)
    {
        sender.TranslationY = 1000;
        sender.IsVisible = true;
        await sender.TranslateTo(0, 0, length: 500, easing: Easing.CubicInOut);
    }
}
