namespace PharmaStoreInventory.Views;

public partial class AnimateBoxes : ContentPage
{
    private double screenWidth = (DeviceDisplay.Current.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density);

    public double ScreenWidth
    {
        get => screenWidth;
    }

    private List<View> _images;
    private int _currentImageIndex = 1;

    public AnimateBoxes()
    {
        InitializeComponent();
        _images = new List<View> { box1, box2, box3, box4, box5 };
        // _images = new List<View> { box5, box4, box3, box2, box1 };

    }

    private async void OnAnimateButtonClicked(object sender, EventArgs e)
    {
        if (_currentImageIndex < _images.Count)
        {
            var currentImage = _images[_currentImageIndex];
            await AnimateFromLeftToRight(currentImage);
            _currentImageIndex++;
        }
    }

    private async void OnAnimateBackwardButtonClicked(object sender, EventArgs e)
    {
        if (_currentImageIndex > 1)
        {
            _currentImageIndex--;
            var currentImage = _images[_currentImageIndex];
            await AnimateFromRightToLeft(currentImage);
        }
    }


    public async Task AnimateFromLeftToRight(View view, uint duration = 500)
    {
        //view.TranslationX = view.Width;
        await view.TranslateTo(0, 0, duration);
    }

    public async Task AnimateFromRightToLeft(View view, uint duration = 500)
    {
        //view.TranslationX = -view.Width;
        await view.TranslateTo(screenWidth, 0, duration);
    }
}