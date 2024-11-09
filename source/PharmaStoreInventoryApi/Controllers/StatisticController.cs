using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace PharmaStoreInventoryApi.Controllers;

[Route("api")]
[ApiController]
public class StatisticController(ProductAmountRepo repo) : ControllerBase
{
    private readonly ProductAmountRepo repo = repo;

    #region Statistics
    [HttpGet("statistic-all")]
    public async Task<IActionResult> GetProductCountsAsync([FromQuery] int storeId)
    {
        var result = await repo.GetProductCountsAsync(storeId);
        return Ok(result);
    }
    
    [HttpGet("statistic1")]
    public async Task<IActionResult> GetCountAllProducts([FromQuery] int storeId)
    {
        var result = await repo.GetCountAllProductsAsync(storeId);
        return Ok(result);
    }

    [HttpGet("statistic2")]
    public async Task<IActionResult> GetCountAllExpiredProducts([FromQuery] int storeId)
    {
        var result = await repo.GetCountAllExpiredProducts(storeId);
        return Ok(result);
    }
    
    [HttpGet("statistic3")]
    public async Task<IActionResult> GetCountAllProductsWillExpireAfter3Months([FromQuery] int storeId)
    {
        var result = await repo.GetCountAllProductsWillExpireAfter3Months(storeId);
        return Ok(result);
    }
    [HttpGet("statistic4")]
    public async Task<IActionResult> GetCountAllIsInventoried([FromQuery] int storeId)
    {
        var result = await repo.GetCountAllIsInventoryed(storeId);
        return Ok(result);
    }
    #endregion
}
