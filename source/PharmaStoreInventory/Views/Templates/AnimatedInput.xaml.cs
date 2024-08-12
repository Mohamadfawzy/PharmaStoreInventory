namespace PharmaStoreInventory.Views.Templates;
using CommunityToolkit.Maui.Core.Platform;
using Microsoft.Maui.Controls;
using PharmaStoreInventory.Helpers;
using PharmaStoreInventory.Validations;

public partial class AnimatedInput : ContentView
{
    public static double BorderHeight => 50.0;

    public enum KeyboardEnum
    {
        Default,
        Text,
        Chat,
        Url,
        Email,
        Telephone,
        Numeric,
    }
    private readonly double height = 25.0;
    private readonly Easing easing = Easing.SinInOut;

    #region BindableProperty

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
        "", BindingMode.TwoWay);

    public static readonly BindableProperty IsErrorProperty =
        BindableProperty.Create(
        nameof(IsError),
        typeof(bool),
        typeof(AnimatedInput),
        false,
        BindingMode.TwoWay,
        propertyChanged: OnIsErrorChanged);

    public static readonly BindableProperty ErrorMessageProperty =
        BindableProperty.Create(
        nameof(ErrorMessage),
        typeof(string),
        typeof(AnimatedInput),
        "Make sure you enter correct values", BindingMode.TwoWay);

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

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(
        nameof(IsPassword),
        typeof(bool),
        typeof(AnimatedInput),
        false, BindingMode.TwoWay);

    public static readonly BindableProperty HasEyeIconProperty =
        BindableProperty.Create(
        nameof(HasEyeIcon),
        typeof(bool),
        typeof(AnimatedInput),
        false, BindingMode.TwoWay);

#pragma warning disable CA2211
    public static BindableProperty KeyboardProperty =
        BindableProperty.Create(
        nameof(EntryKeyboard),
        typeof(KeyboardEnum),
        typeof(AnimatedInput),
        KeyboardEnum.Default, BindingMode.TwoWay);
#pragma warning restore CA2211

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

    public bool IsError
    {
        get => (bool)GetValue(IsErrorProperty);
        set => SetValue(IsErrorProperty, value);
    }

    public string ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
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

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public bool HasEyeIcon
    {
        get => (bool)GetValue(HasEyeIconProperty);
        set => SetValue(HasEyeIconProperty, value);
    }

    public KeyboardEnum EntryKeyboard
    {
        get => (KeyboardEnum)GetValue(KeyboardProperty);
        set
        {
            SetValue(KeyboardProperty, value);
            SetKeyboard();
        }
    }
    #endregion

    private static void OnIsErrorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AnimatedInput)bindable;
        var isError = (bool)newValue;
        control.UpdateErrorState(isError);
    }

    private void UpdateErrorState(bool isError)
    {
        if (isError)
        {
            if (!error.IsVisible)
            {
                error.IsVisible = true;
                AnimateErrorMessage(0, height, 0, 1); // Adjust the height as needed
            }
        }
        else
        {
            AnimateErrorMessage(height, 0, 1, 0); // Adjust the height as needed
        }
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
        entry.Unfocus();
        entry.HideSoftInputAsync(CancellationToken.None);
        //        if (entry.IsFocused)
        //        {
        //#if ANDROID
        //            entry.Unfocus();
        //            entry.HideKeyboardAsync(CancellationToken.None);
        //#endif
        //        }
        return true;
    }

    public bool EntryIsFocused() => entry.IsFocused;



    private async void Entry_Focused(object sender, FocusEventArgs e)
    {
        SetFocusColors();
        await AnimatePlaceholder(CalculateTranslationX(), CalculateTranslationY(), 0.8);
    }

    private async void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        if (string.IsNullOrEmpty(entry.Text))
        {
            SetUnFocusColors();
            await AnimatePlaceholder();
        }
        Validation();
    }

    private void Validation()
    {
        if (EntryKeyboard == KeyboardEnum.Email)
        {
            if (!Validator.IsValidEmail(InputText))
                IsError = true;
        }
        if (EntryKeyboard == KeyboardEnum.Telephone)
        {
            if (!Validator.IsValidTelephone(InputText))
                IsError = true;
        }
    }

    private async void AnimateErrorMessage(double fromHeight, double toHeight, double fromOpacity, double toOpacity)
    {
        try
        {
            await Task.Run(() =>
            {
                new Animation
                {
                    { 0, 1, new Animation (v => error.HeightRequest = v,fromHeight, toHeight) },
                    { 0, 1, new Animation (v => error.Opacity = v, fromOpacity ,toOpacity )}
                }.Commit(this, "InvalidMessage", 16, 500, easing: easing, (v, c) =>
                {
                    if (toOpacity == 0)
                        error.IsVisible = false;
                });
            });
        }
        catch (Exception ex)
        {
            error.IsVisible = false;
            await Helpers.Alerts.DisplaySnackbar(ex.Message);
        }
    }

    private void TogglePasswordVisibility(object sender, TappedEventArgs e)
    {
        IsPassword = !IsPassword;

        if (IsPassword)
        {
            eyeIcon.Text = DrasatHealthMobile.Helpers.IconFont.EyeOutline; // Set to eye icon
        }
        else
        {
            eyeIcon.Text = DrasatHealthMobile.Helpers.IconFont.EyeOffOutline; // Set to eye-off icon
        }
    }

    private async Task AnimatePlaceholder(double translationX = 0, double translationY = 0, double scale = 1)
    {
        var scaleTask = placeholder.ScaleTo(scale, (uint)TranslateDuration, easing);
        var translateTask = placeholder.TranslateTo(translationX, translationY, (uint)TranslateDuration, easing);
        await Task.WhenAll(scaleTask, translateTask);
    }

    private async void PositioningOfPlaceHolder()
    {
        if (!string.IsNullOrEmpty(InputText)) // InputText is have value
        {
            await Task.Delay(300);

            await Task.Run(() =>
            {
                SetFocusColors();
                placeholder.Scale = 0.8;
                placeholder.TranslationX = CalculateTranslationX();
                placeholder.TranslationY = CalculateTranslationY();
            });
        }
    }

    public void ClearText()
    {
        InputText = string.Empty;
        SetUnFocusColors();
        placeholder.TranslationX = 0; 
        placeholder.TranslationY = 0; 
        placeholder.Scale = 1;
    }
    private double CalculateTranslationX()
    {
        //return container.FlowDirection == FlowDirection.RightToLeft ? 10 : -10;

        if (AppValues.Language == "ar")
        {
            return 10;
        }
        else return  -10;
        //return container.FlowDirection == FlowDirection.RightToLeft ? 10 : -10;
    }

    private double CalculateTranslationY()
    {
        return BorderHeight / -2;
    }

    private void SetFocusColors()
    {
        border.Stroke = FocusColor;
        placeholder.TextColor = FocusColor;
        eyeIcon.TextColor = FocusColor;
    }

    private void SetUnFocusColors()
    {
        placeholder.TextColor = UnFocusColor;
        border.Stroke = UnFocusColor;
        eyeIcon.TextColor = UnFocusColor;
    }

    private void InputTextChanged(object sender, TextChangedEventArgs e)
    {
        if (IsError) IsError = false;
        //PositioningOfPlaceHolder();
    }

    private void SetKeyboard()
    {
        entry.Keyboard = EntryKeyboard switch
        {
            KeyboardEnum.Default => Keyboard.Default,
            KeyboardEnum.Text => Keyboard.Text,
            KeyboardEnum.Chat => Keyboard.Chat,
            KeyboardEnum.Url => Keyboard.Url,
            KeyboardEnum.Email => Keyboard.Email,
            KeyboardEnum.Telephone => Keyboard.Telephone,
            KeyboardEnum.Numeric => Keyboard.Numeric,
            _ => Keyboard.Default,
        };
    }
}