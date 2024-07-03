using CommunityToolkit.Mvvm.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PharmaStoreInventory.Models;

public class StoresModel : ObservableObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set => SetProperty(ref isSelected, value);
    }
}
