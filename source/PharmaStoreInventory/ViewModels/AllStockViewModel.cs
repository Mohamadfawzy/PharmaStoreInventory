using PharmaStoreInventory.Models;

namespace PharmaStoreInventory.ViewModels;

public class AllStockViewModel
{
    public List<StockModel> StockModelList { get; set; } = null!;
    public AllStockViewModel()
    {
        GetStockModelList();
    }

    void GetStockModelList()
    {
        StockModelList = new List<StockModel>
        {
            new StockModel
            {
                ItemNameArabic = "باراسيتامول",
                ItemNameEnglish = "Kenagel silicone gel 30 gm",
                Quantity = 90000150,
                Price = 515.75m,
                ExpiryDate = new DateOnly(2025, 12, 31),
                IsCounted = true,
                Distributor = "شركة الأدوية الحديثة"
            },
            new StockModel
            {
                ItemNameArabic = "أموكسيسيلين",
                ItemNameEnglish = "Amoxicillin",
                Quantity = 50,
                Price = 25.00m,
                ExpiryDate = new DateOnly(2024, 6, 30),
                IsCounted = false,
                Distributor = "شركة الدواء العربي"
            },
            new StockModel
            {
                ItemNameArabic = "إيبوبروفين",
                ItemNameEnglish = "Ibuprofen",
                Quantity = 75,
                Price = 20.00m,
                ExpiryDate = new DateOnly(2024, 11, 30),
                IsCounted = false,
                Distributor = "شركة الشفاء الدوائية"
            },
            new StockModel
            {
                ItemNameArabic = "ميترونيدازول",
                ItemNameEnglish = "Metronidazole",
                Quantity = 30,
                Price = 10.00m,
                ExpiryDate = new DateOnly(2023, 8, 31),
                IsCounted = true,
                Distributor = "شركة الصحة العالمية"
            },
            new StockModel
            {
                ItemNameArabic = "كلاريثروميسين",
                ItemNameEnglish = "Clarithromycin",
                Quantity = 20,
                Price = 40.00m,
                ExpiryDate = new DateOnly(2024, 2, 28),
                IsCounted = false,
                Distributor = "شركة الأمل للصناعات الدوائية"
            }
        };
    }
}
