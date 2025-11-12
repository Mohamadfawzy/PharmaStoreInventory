using Microsoft.Maui.Controls.Shapes;
using PharmaStoreInventory.Helpers;

namespace PharmaStoreInventory.Views.Templates;

public partial class DashboardNumberTemplate : ContentView
{
    public static readonly BindableProperty NumberProperty =
    BindableProperty.Create(
        nameof(Number),
        typeof(int),
        typeof(DashboardNumberTemplate),
        0, BindingMode.TwoWay);

    public int Number
    {
        get => (int)GetValue(NumberProperty);
        set => SetValue(NumberProperty, value);
    }
    
    public static readonly BindableProperty TitleProperty =
    BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(DashboardNumberTemplate),
        "", BindingMode.TwoWay);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }


    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(DashboardNumberTemplate),
            Colors.Transparent);

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    public DashboardNumberTemplate()
	{
		InitializeComponent();
        //if (AppValues.Language == "ar")
        //{
        //    border.StrokeShape = new RoundRectangle
        //    {
        //        CornerRadius = new CornerRadius(50, 6, 6, 6)
        //    };
        //}
        //else
        //{
        //    border.StrokeShape = new RoundRectangle
        //    {
        //        CornerRadius = new CornerRadius(6, 50, 6, 6)
        //    };
        //}
    }
}