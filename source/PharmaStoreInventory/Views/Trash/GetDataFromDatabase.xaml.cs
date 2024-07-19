using PharmaStoreInventory.Services;

namespace PharmaStoreInventory.Views.Trash;

public partial class GetDataFromDatabase : ContentPage
{
	public GetDataFromDatabase()
	{
		InitializeComponent();

        string srvrdbname = "stock";
        string srvrname = "192.168.1.2";

        string sqlconn = $"Data Source={srvrname}.1433;Initial Catalog={srvrdbname};;Integrated Security=True; Trust Server Certificate=True";

        //_databaseService = new DatabaseService("Data Source=MOSOFT\\MSSQLSERVER01;Integrated Security=True;Trust Server Certificate=True");
        _databaseService = new DatabaseService(sqlconn);
        
        LoadData();
    }

    private readonly DatabaseService _databaseService;


    private async void LoadData()
    {
        string query = "SELECT top(10) product_id, buy_price FROM Product_Amount";
        var dataTable = await _databaseService.GetDataAsync(query);
        var row = dataTable.Rows;
    }
}