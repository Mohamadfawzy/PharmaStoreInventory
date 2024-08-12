using DataAccess.Services;
using PharmaStoreInventory.Models;
using System.Text.Json;
namespace PharmaStoreInventory.Views;
public partial class BranchesView : ContentPage
{
    //private readonly string _fileName = FileSystem.AppDataDirectory + "/branch.json";
    readonly FileHanler FileHanler;
    public BranchesView()
    {
        InitializeComponent();
        FileHanler = new(Helpers.AppValues.BranchsFileName);
        ReadFromFile();
    }
    private void BtnContact(object sender, EventArgs e)
    {

    }

    private async void ReadFromFile()
    {
        var branchs = await FileHanler.ReadFromFile<Branch>();
        clBranches.ItemsSource = branchs;
        //container.Clear();
        //if (branchs != null)
        //{
        //    foreach (var item in branchs)
        //    {
        //        var label = new Label()
        //        {
        //            Text = item.BrachName,
        //        };
        //        container.Children.Add(label);
        //    }
        //}
    }







    private void Button_Clicked_2(object sender, EventArgs e)
    {
        //list.Add(new Branch() { BrachName = new Random().Next(999999).ToString() });
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var b = new Branch() { BrachName = new Random().Next(999999).ToString() };
        await FileHanler.Add<Branch>(b);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        ReadFromFile();
    }


}