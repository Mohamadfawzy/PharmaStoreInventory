using CommunityToolkit.Mvvm.ComponentModel;

namespace PharmaStoreInventory.Models;

public class SortModel : ObservableObject
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;

    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set => SetProperty(ref isSelected, value);
    }
}
