using DataAccess.Entities;
namespace DataAccess.Dtos;

public class ProductDetailsDto 
{

    public decimal? ExpiryGroupID { get; set; } //Counter_id
    public decimal Id { get; set; }
    public decimal ProductId { get; set; }
    public decimal? StoreId { get; set; }
    public string? IsInventoried { get; set; }
    //public decimal? VendorId { get; set; }
    public DateTime? ExpDate { get; set; }
    public decimal? Quantity{ get; set; } // Amount
    public decimal? SellPrice { get; set; }
    public string? ProductNameAr { get; set; }
    public string? ProductNameEn { get; set; }
    public decimal? ProductUnit1 { get; set; }
    //public decimal? ProductUnit13 { get; set; }
    public string? VendorNameAr { get; set; }
    //public string? CompanyNameAr { get; set; }
    public string? ProductHasExpire { get; set; }

    //public event NotifyCollectionChangedEventHandler? CollectionChanged;

    //public event PropertyChangedEventHandler PropertyChanged;
    //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    //{
    //    var changed = PropertyChanged;
    //    if (changed == null)
    //        return;

    //    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}
    //protected bool SetProperty<T>(ref T backingStore, T value,
    //                                [CallerMemberName] string propertyName = "",
    //                                Action onChanged = null)
    //{
    //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
    //        return false;

    //    backingStore = value;
    //    onChanged?.Invoke();
    //    OnPropertyChanged(propertyName);
    //    return true;
    //}

    public static implicit operator ProductDetailsDto?(Product_Amount? item)
    {
        if (item == null) 
            return null;
        return new ProductDetailsDto
        {
            Id = item.Pa_id,
            ProductId = item.Product_id,
            StoreId = item.Store_id,
            ExpiryGroupID = item.Counter_id,
            //VendorId = item.Vendor_id,
            ExpDate = item.Exp_date,
            Quantity = item.Amount,
            SellPrice = item.Sell_price,
            IsInventoried = item.Product_update,
            // Map other properties as needed
        };
    }
}


