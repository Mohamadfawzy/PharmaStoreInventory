namespace PharmaStoreInventory.Views.Templates;
using CommunityToolkit.Maui.Core.Platform;
using Microsoft.Maui.Controls;
using System.Xml.Linq;

public partial class AnimatedInput : ContentView
{
    private uint length = 250;
    private Easing easing = Easing.SinInOut;
    public double borderHeight => 50.0;

    public static readonly BindableProperty UnFocusColorProperty =
        BindableProperty.Create(
        nameof(UnFocusColor),
        typeof(Color),
        typeof(AnimatedInput),
        default, BindingMode.OneWay);

    public static readonly BindableProperty FocusColorProperty =
    BindableProperty.Create(
        nameof(FocusColor),
        typeof(Color),
        typeof(AnimatedInput),
        default, BindingMode.OneWay);

    public static readonly BindableProperty InputBackgroundColorProperty =
    BindableProperty.Create(
        nameof(InputBackgroundColor),
        typeof(Color),
        typeof(AnimatedInput),
        Colors.White, BindingMode.OneTime);

    public static readonly BindableProperty InputTextProperty =
        BindableProperty.Create(
        nameof(InputText),
        typeof(string),
        typeof(AnimatedInput),
        null, BindingMode.TwoWay);

    public static readonly BindableProperty InvalidInputProperty =
    BindableProperty.Create(
        nameof(InvalidInput),
        typeof(bool),
        typeof(AnimatedInput),
        false, BindingMode.OneWay);

    public static readonly BindableProperty InvalidMessageProperty =
        BindableProperty.Create(
        nameof(InvalidMessage),
        typeof(string),
        typeof(AnimatedInput),
        null, BindingMode.TwoWay);

    public static readonly BindableProperty TranslateDurationProperty =
    BindableProperty.Create(
        nameof(TranslateDuration),
        typeof(int),
        typeof(AnimatedInput),
        250, BindingMode.TwoWay);

    public static readonly BindableProperty InputPlaceholderProperty =
        BindableProperty.Create(
        nameof(InputPlaceholder),
        typeof(string),
        typeof(AnimatedInput),
        default, BindingMode.TwoWay);


    public Color UnFocusColor
    {
        get => (Color)GetValue(UnFocusColorProperty);
        set => SetValue(UnFocusColorProperty, value);
    }

    public Color FocusColor
    {
        get => (Color)GetValue(FocusColorProperty);
        set => SetValue(FocusColorProperty, value);
    }

    public Color InputBackgroundColor
    {
        get => (Color)GetValue(InputBackgroundColorProperty);
        set => SetValue(InputBackgroundColorProperty, value);
    }

    public string InputText
    {
        get => (string)GetValue(InputTextProperty);
        set => SetValue(InputTextProperty, value);
    }

    public bool InvalidInput
    {
        get => (bool)GetValue(InvalidInputProperty);
        set => SetValue(InvalidInputProperty, value);
    }

    public string InvalidMessage
    {
        get => (string)GetValue(InvalidMessageProperty);
        set => SetValue(InvalidMessageProperty, value);
    }

    public int TranslateDuration
    {
        get => (int)GetValue(TranslateDurationProperty);
        set => SetValue(TranslateDurationProperty, value);
    }

    public string InputPlaceholder
    {
        get => (string)GetValue(InputPlaceholderProperty);
        set => SetValue(InputPlaceholderProperty, value);
    }

    // CONSTRUCTOR
    public AnimatedInput()
    {
        InitializeComponent();

    }

    protected override void OnParentSet()
    {
        base.OnParentSet();
        PositioningOfPlaceHolder();
    }

    public bool HideKeyBoard()
    {
        if (entry.IsFocused)
        {
            entry.Unfocus();
            entry.HideKeyboardAsync(CancellationToken.None);
        }
        return true;
    }
    public bool EntryIsFocused() => entry.IsFocused;
    private async void Entry_Focused(object sender, FocusEventArgs e)
    {
        SetFocusColors();
        //InvalidInput = false;
        await AnimateLabel(CalculateTranslationX(), CalculateTranslationY(), 0.8);
    }

    private async void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(entry.Text))
        {
            await AnimateLabel();
            SetUnFocusColors();
        }
    }

    private async Task AnimateLabel(double translationX = 0, double translationY = 0, double scale = 1)
    {
        var scaleTask = label.ScaleTo(scale, (uint)TranslateDuration, easing);
        var translateTask = label.TranslateTo(translationX, translationY, (uint)TranslateDuration, easing);
        await Task.WhenAll(scaleTask, translateTask);
    }

    private async void PositioningOfPlaceHolder()
    {
        await Task.Run(async () =>
        {
            await Task.Delay(300);
            if (!string.IsNullOrEmpty(InputText))
            {
                SetFocusColors();
                label.Scale = 0.8;
                label.TranslationX = CalculateTranslationX();
                label.TranslationY = CalculateTranslationY();
            }
        });
    }

    private double CalculateTranslationX()
    {
        return container.FlowDirection == FlowDirection.RightToLeft ? 10 : -10;
    }

    private double CalculateTranslationY()
    {
        return borderHeight / -2;
    }

    private void SetFocusColors()
    {
        border.Stroke = FocusColor;
        label.TextColor = FocusColor;
    }

    private void SetUnFocusColors()
    {
        label.TextColor = UnFocusColor;
        border.Stroke = UnFocusColor;
    }

    private void entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (InvalidInput) InvalidInput = false;
    }
}