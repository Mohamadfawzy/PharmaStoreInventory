﻿namespace DataAccess.Dtos;

public class ProductDto
{
    //public decimal ProductId { get; set; }
    public string? ProductCode { get; set; }
    public string? InternationalCode { get; set; }
    public string? Name { get; set; }
    public decimal? SalePrice { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? StoreId { get; set; }
}
