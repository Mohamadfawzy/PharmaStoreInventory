using AdminApp.ViewModels;

namespace AdminApp.Views;

public partial class UsersView : ContentPage
{
    UsersViewModel vm;
    public UsersView()
    {
        InitializeComponent();
        vm = BindingContext as UsersViewModel;
    }

    private async void GoToUserDetailsViewTapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushAsync(new UserDetailsView());
    }
    private void HamburgerTapped(object sender, TappedEventArgs e)
    {
        //Navigation.PushAsync(new UserView());
    }
    double currentOffset = 0;
    private async void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (e.VerticalOffset < currentOffset)
            return;
        if (e.LastVisibleItemIndex == vm.Users.Count - 5)
        {
            await vm.FetchNextPageOfUsersAsync();
        }
        currentOffset = e.VerticalOffset;
    }
}