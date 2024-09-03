using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Dtos;

namespace PharmaStoreInventory.Models;

public class ProductDetailsModel : ObservableObject
{
    decimal? quantity;
    string? isInventoried;
    private DateTime? expDate;

    public decimal Id { get; set; }
    public decimal ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public decimal? ExpiryGroupID { get; set; }
    public string? IsInventoried { get => isInventoried; set => SetProperty(ref isInventoried, value); }
    public DateTime? ExpDate { get => expDate; set => SetProperty(ref expDate, value); }
    public decimal? Quantity { get => quantity; set => SetProperty(ref quantity, value); }
    public decimal? SellPrice { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductNameEn { get; set; }
    public decimal? ProductUnit1 { get; set; }
    public decimal? ProductUnit13 { get; set; }
    public string? VendorNameAr { get; set; }
    public string? CompanyNameAr { get; set; }

    public static implicit operator ProductDetailsModel(ProductDetailsDto? item)
    {
        if (item == null)
        {
            return default(ProductDetailsDto);
        }

        return new ProductDetailsModel
        {

            Id = item.Id,
            ProductId = item.ProductId,
            StoreId = item.StoreId,
            Quantity = item.Quantity,
            ExpiryGroupID = item.ExpiryGroupID,
            ProductNameAr = item.ProductNameAr,
            ProductNameEn = item.ProductNameEn,
            ProductUnit1 = item.ProductUnit1,
            ProductUnit13 = item.ProductUnit13,
            VendorNameAr = item.VendorNameAr,
            CompanyNameAr = item.CompanyNameAr,
            ExpDate = item.ExpDate,
            IsInventoried = item.IsInventoried,
            SellPrice = item.SellPrice,

        };
    }

    public static IEnumerable<ProductDetailsModel> ToList1(IEnumerable<ProductDetailsDto> list)
    {
        //var newList = new List<ProductDetailsModel>();
        foreach (var item in list)
        {
            yield return item;
        }
        // return newList;
    }

}
