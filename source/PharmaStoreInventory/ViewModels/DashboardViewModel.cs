using CommunityToolkit.Mvvm.ComponentModel;
using PharmaStoreInventory.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace PharmaStoreInventory.ViewModels;

public class DashboardViewModel : ObservableObject
{

    private string title = "العنوان";
    public string Title
    {
        get => title;
        set => SetProperty(ref title, value);
    }

    private string number = "124568";
    public string Number
    {
        get => number;
        set => SetProperty(ref number, value);
    }

    public List<StoresModel> StoresModelList { get; set; }
    public ICommand StoreSelectionChangedCommand => new Command<StoresModel>(StoreSelectionChanged);


    public DashboardViewModel()
    {
        StoresModelList = new List<StoresModel>();
        GetStockModelList();
    }


    void GetStockModelList()
    {
        var list = Services.MockData.StoresModelList;

        foreach (var model in list)
        {
            if (model != null)
            {
                StoresModelList.Add(model);
            }
        }
    }

    void StoreSelectionChanged(StoresModel item)
    {
        var oldItem = StoresModelList.First(c => c.IsSelected);
        oldItem.IsSelected = false;
        item.IsSelected = true;
    }
}

public class StatisticsResultsModel
{
    public string Title { get; set; }
    public string Number { get; set; }

    public StatisticsResultsModel(string title, string number)
    {
        Title = title;
        Number = number;
    }
}
