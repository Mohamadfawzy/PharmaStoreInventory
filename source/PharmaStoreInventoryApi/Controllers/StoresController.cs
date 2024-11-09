using DataAccess.Dtos;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PharmaStoreInventoryApi.Controllers;

[Route("api/commons")]
[ApiController]
public class StoresController(CommonsRepo commonsRepo) : ControllerBase
{
    private readonly CommonsRepo repo = commonsRepo;

    [HttpGet("stores")]
    public async Task<IActionResult> Stores()
    {
        return Ok(await repo.GetAllStores());
    }

    // أحدث تاريخ للمخزون
    [HttpGet("latest_inventory_history")]
    public async Task<IActionResult> GetLatestInventoryHistory([FromQuery] int sotreId)
    {
        return Ok(await repo.GetLatestInventoryHistoryAsync(sotreId));
    }
    
    [HttpPost("start_new_inventory")]
    public async Task<IActionResult> StartNewInventory([FromBody] StartNewInventoryHistoryDto model)
    {
        return Ok(await repo.StartNewInventoryAsync(model));
    }
}
