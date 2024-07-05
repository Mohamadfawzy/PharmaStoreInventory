using PharmaStoreInventory.Models;
using System.Linq;
namespace PharmaStoreInventory.Services;

public static class MockData
{
    public static List<StockModel> StockModelList = new List<StockModel>
        {
            new StockModel
            {
                Id = 1,
                ItemNameArabic = "سبكتون",
                ItemNameEnglish = "Spectone",
                Quantity = 36,
                Price = 27.25m,
                ExpiryDate = new DateOnly(2026, 11, 1),
                IsCounted = true,
                Distributor = "شركة القاهرة للأدوية والصناعات الكيماوية",
                Barcode = "6221068000977"

            },
        
          new StockModel
            {
                Id = 1,
                ItemNameArabic = "سبكتون",
                ItemNameEnglish = "Spectone",
                Quantity = 36,
                Price = 27.25m,
                ExpiryDate = new DateOnly(2026, 11, 1),
                IsCounted = true,
                Distributor = "شركة القاهرة للأدوية والصناعات الكيماوية",
                Barcode = "6221068000977"

            },
            new StockModel
            {
                Id = 2,
                ItemNameArabic = "كال-ماج",
                ItemNameEnglish = "CAL-MAG",
                Quantity = 40,
                Price = 22.25m,
                ExpiryDate = new DateOnly(2025, 10, 1),
                IsCounted = true,
                Distributor = "Hochster",
                Barcode = "6224007751008"
            },
            new StockModel
            {
                Id= 3,
                ItemNameArabic = "إديمكس",
                ItemNameEnglish = "Edemex",
                Quantity = 90000150,
                Price = 515.75m,
                ExpiryDate = new DateOnly(2025, 12, 31),
                IsCounted = true,
                Distributor = "شركة الأدوية الحديثة",
                Barcode = "6221050030074"
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
            },

            // dublcated
            new StockModel
            {
                Id = 11,
                ItemNameArabic = "سبكتون",
                ItemNameEnglish = "Spectone",
                Quantity = 7,
                Price = 28.25m,
                ExpiryDate = new DateOnly(2026, 8, 1),
                IsCounted = true,
                Distributor = "شركة القاهرة للأدوية والصناعات الكيماوية",
                Barcode = "6221068000977"
            },
            new StockModel
            {
                Id = 12,
                ItemNameArabic = "كال-ماج",
                ItemNameEnglish = "CAL-MAG",
                Quantity = 5,
                Price = 23.25m,
                ExpiryDate = new DateOnly(2025, 3, 1),
                IsCounted = true,
                Distributor = "Hochster",
                Barcode = "6224007751008"
            },
            new StockModel
            {
                Id= 13,
                ItemNameArabic = "إديمكس",
                ItemNameEnglish = "Edemex",
                Quantity = 15,
                Price = 516.75m,
                ExpiryDate = new DateOnly(2025, 7, 31),
                IsCounted = true,
                Distributor = "شركة الأدوية الحديثة",
                Barcode = "6221050030074"
            },
        };

    public static List<StoresModel> StoresModelList = new List<StoresModel>
    {
        new StoresModel { Id = 2, IsSelected = false , Name="المخزن الخارجي", },
        new StoresModel { Id = 3, IsSelected = false , Name="مخزن شبين"},
        new StoresModel { Id = 1, IsSelected = true , Name="الصيدلية"},
        new StoresModel { Id = 4, IsSelected = false , Name="مخزن طنطا"},
        
        //new StoresModel { Id = 2, IsSelected = false , Name="المخزن الخارجي"},
        //new StoresModel { Id = 3, IsSelected = false , Name="مخزن شبين"},
        //new StoresModel { Id = 1, IsSelected = true , Name="الصيدلية"},
        //new StoresModel { Id = 4, IsSelected = false , Name="مخزن طنطا"},
        
        
        //new StoresModel { Id = 2, IsSelected = false , Name="المخزن الخارجي"},
        //new StoresModel { Id = 3, IsSelected = false , Name="مخزن شبين"},
        //new StoresModel { Id = 1, IsSelected = true , Name="الصيدلية"},
        //new StoresModel { Id = 4, IsSelected = false , Name="مخزن طنطا"},
        
        //new StoresModel { Id = 2, IsSelected = false , Name="المخزن الخارجي"},
        //new StoresModel { Id = 3, IsSelected = false , Name="مخزن شبين"},
        //new StoresModel { Id = 1, IsSelected = true , Name="الصيدلية"},
        //new StoresModel { Id = 4, IsSelected = false , Name="مخزن طنطا"},
        
        //new StoresModel { Id = 2, IsSelected = false , Name="المخزن الخارجي"},
        //new StoresModel { Id = 3, IsSelected = false , Name="مخزن شبين"},
        //new StoresModel { Id = 1, IsSelected = true , Name="الصيدلية"},
        //new StoresModel { Id = 4, IsSelected = false , Name="مخزن طنطا"},
        
        
        //new StoresModel { Id = 2, IsSelected = false , Name="المخزن الخارجي"},
        //new StoresModel { Id = 3, IsSelected = false , Name="مخزن شبين"},
        //new StoresModel { Id = 1, IsSelected = true , Name="الصيدلية"},
        //new StoresModel { Id = 4, IsSelected = false , Name="مخزن طنطا"}
    };
    public static List<StockModel>  GetStocks()
    {
        return StockModelList.ToList();
    }
    
    public static List<StockModel>  GetStocksNonRepet()
    {
        return StockModelList.DistinctBy(i=>i.Barcode).ToList();
    }
    
    public static List<StockModel>?  GetStocksByText(string text)
    {
        return StockModelList.Where(x=>x.ItemNameEnglish.ToLower().Contains(text)).ToList();
    }
    
    public static List<StockModel>  GetStockByBarcode(string code)
    {
        return StockModelList.Where(x => x.Barcode == code).ToList();
    }
    
    
    public static List<StoresModel> GetStores(string code)
    {
        return StoresModelList.ToList();
    }
}
