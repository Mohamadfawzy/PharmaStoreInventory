namespace PharmaStoreInventory.Models;

public class NoDataModel(string? image, string? title, string? description, bool isTryVisible,bool button2Visibl = false)
{
    public string? Image { get; set; } = image;
    public string? Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public bool IsTryVisibl { get; set; } = isTryVisible;
    public bool Button2Visibl { get; set; } = button2Visibl;
}
