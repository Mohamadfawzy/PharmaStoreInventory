using Microsoft.Maui.Controls.Shapes;

namespace PharmaStoreInventory.Views.Trash;

public partial class AnimateBoxes : ContentPage
{
    private readonly double  screenWidth = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);

    public double ScreenWidth
    {
        get => screenWidth;
    }

    private readonly List<View> element;
    private int _currentImageIndex = 1;

    public AnimateBoxes()
    {
        InitializeComponent();
        element = [box1, box2, box3, box4, box5];
        // element = new List<View> { box5, box4, box3, box2, box1 };

    }

    private async void OnAnimateButtonClicked(object sender, EventArgs e)
    {
        if (_currentImageIndex < element.Count)
        {
            var currentImage = element[_currentImageIndex];
            await AnimateFromLeftToRight(currentImage);
            _currentImageIndex++;
        }
    }

    private async void OnAnimateBackwardButtonClicked(object sender, EventArgs e)
    {
        if (_currentImageIndex > 1)
        {
            _currentImageIndex--;
            var currentImage = element[_currentImageIndex];
            await AnimateFromRightToLeft(currentImage);
        }
    }


    private async static Task AnimateFromLeftToRight(View view, uint duration = 250)
    {
        //view.TranslationX = view.Width;
        await view.TranslateTo(0, 0, duration);
    }

    private async Task AnimateFromRightToLeft(View view, uint duration = 250)
    {
        //view.TranslationX = -view.Width;
        await view.TranslateTo(screenWidth, 0, duration);
    }

}