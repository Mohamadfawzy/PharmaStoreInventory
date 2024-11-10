using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace PharmaStoreInventoryApi.Controllers;

[Route("api/system-version")]
[ApiController]
public class SystemVersionController(CommonsRepo commonsRepo) : ControllerBase
{
    private readonly CommonsRepo repo = commonsRepo;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await repo.GetCurrentSystemVersion());
    }
}
