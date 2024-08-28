using DataAccess.DomainModel;
using DataAccess.Services;
using PharmaStoreInventory.Models;
using System.Text.Json;
namespace PharmaStoreInventory.Views;
public partial class BranchesView : ContentPage
{
    //private readonly string _fileName = FileSystem.AppDataDirectory + "/branch.json";
    readonly JsonFileHanler FileHanler;
    readonly XmlFileHandler XFileHanler;
    public BranchesView()
    {
        InitializeComponent();
        FileHanler = new(Helpers.AppValues.BranchsFileName);
        XFileHanler = new(Helpers.AppValues.XBranchsFileName);
        GetAllBranchs();
        //ReadFromFile();
    }
    private void BtnContact(object sender, EventArgs e)
    {

    }
    private async void GetAllBranchs()
    {
        var branches = await XFileHanler.All();
        clBranches.ItemsSource = branches;
    }
    private async void ReadFromFile()
    {
        var branches = await FileHanler.ReadFromFile<Branch>();
        clBranches.ItemsSource = branches;
    }

    private void OnDoneSwipeItemInvoked(object sender, EventArgs e)
    {

    }


    private void SwipeItemView_Invoked(object sender, EventArgs e)
    {

    }

    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            var item = btn.CommandParameter as BranchModel;
            if (item != null)
            {
                await XFileHanler.RemoveById(item.Id.ToString());
                GetAllBranchs();
            }
        }
    }
}