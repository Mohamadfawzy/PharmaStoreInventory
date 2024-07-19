using DataAccess.Helper;
using PharmaStoreInventory.Models;
using System.Text.Json;
namespace PharmaStoreInventory.Views;
public partial class BranchesView : ContentPage
{
    public List<Branch> List { get; set; } = new List<Branch>
    { 
        new Branch() { BrachName = "name of brans" },
        new Branch() { BrachName = "name of brans" },
        new Branch() { BrachName = "name of brans" },
        new Branch() { BrachName = "name of brans" },
    };
    //private readonly string _fileName = FileSystem.AppDataDirectory + "/branch.json";
    readonly FileHanler FileHanler;
    public BranchesView()
    {
        InitializeComponent();
        FileHanler = new FileHanler();
        ReadFromFile();
    }
    private void BtnContact(object sender, EventArgs e)
    {

    }

    private async void ReadFromFile()
    {
        var branchs = await FileHanler.ReadFromFile<Branch>(Helpers.AppConstants.BranchsFileName);
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
        await FileHanler.Add<Branch>(b, Helpers.AppConstants.BranchsFileName);
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        ReadFromFile();
    }


}