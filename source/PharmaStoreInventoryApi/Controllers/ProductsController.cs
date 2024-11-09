using DataAccess.DomainModel;
using DataAccess.DomainModel.QueryParams;
using DataAccess.Dtos;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace PharmaStoreAPI.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{

    private readonly ProductAmountRepo repo;

    public ProductsController(ProductAmountRepo repo)
    {
        this.repo = repo;
    }
    [AllowAnonymous]
    [HttpGet("all")]
    public async Task<IActionResult> GetProducts([FromQuery] ProductQParam query)
    {
        var result = await repo.GetAllProducts(query);
        return Ok(result);
    }

    [HttpGet("product-details")]
    public async Task<IActionResult> ProductDetails([FromQuery] bool hasOnlyQuantity, string barcode, int storeId)
    {
        var result = await repo.GetProductDetails(hasOnlyQuantity, barcode, storeId);
        return Ok(result);
    }

    [HttpPut("edit-inventory-status")]
    public async Task<IActionResult> UpdateInventoryStatus([FromQuery] bool allOneTime, string status, int productId, int expiryBatchID)
    {
        var result = await repo.UpdateInventoryStatus(allOneTime, status, productId, expiryBatchID);
        return Ok(result);
    }

    [HttpPut("edit-quantity")]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateProductQuantityDto model)
    {
        Result res = await repo.CanAddExpirationDate((int)model.Id, model.ProductId, model.ExpDate);
        if (res.IsSuccess)
        {
            res = await repo.UpdateQuantity(model);
        }
        return Ok(res);
    }

    [HttpPost("copy")]
    public async Task<IActionResult> CopyProductAmout([FromBody] CopyProductAmoutDto model)
    {
        var res = await repo.CopyProductAmout(model);
        return Ok(res);
    }

}



