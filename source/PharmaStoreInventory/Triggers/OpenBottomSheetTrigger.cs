namespace PharmaStoreInventory.Triggers;

public class OpenBottomSheetTrigger : TriggerAction<VisualElement>
{
    public int StartsFrom { get; set; }

    protected async override void Invoke(VisualElement sender)
    {
        sender.TranslationY = 1000;
        sender.IsVisible = true;
        await sender.TranslateTo(0, 0, length: 500, easing: Easing.CubicInOut);

        //sender.Animate("FadeTriggerAction", new Animation((d) =>
        //{
        //    //var val = StartsFrom == 1 ? d : 1 - d;
        //    //sender.BackgroundColor = Color.FromRgb(1, val, 1);
        //    sender.TranslationY = 0;
        //}),
        //length: 1000, // milliseconds
        //easing: Easing.CubicInOut);
    }
}
