using CommunityToolkit.Mvvm.ComponentModel;

namespace AdminApp.Models;

public class FilterOption : ObservableObject
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;

    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set => SetProperty(ref isSelected, value);
    }
}
