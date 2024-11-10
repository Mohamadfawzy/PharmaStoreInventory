using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Contexts;
using DataAccess.DomainModel;

namespace DataAccess.Repository;

public class CommonsRepo(AppDb context)
{
    private readonly AppDb context = context;

    public async Task<List<Stores>?> GetAllStores()
    {
        return await context.Stores.ToListAsync();
    }

    public async Task<InventoryHistory?> GetLatestInventoryHistoryAsync(int storeId)
    {
        try
        {
            var latestInventory = await context.InventoryHistory // أحدث جرد
            .Where(s => s.Start_time != null) // Exclude null dates
            .Where(s => s.Store_id == storeId)
            .OrderByDescending(o => o.Start_time)
            .Select(history => new InventoryHistory()
            {
                Ssut_id = history.Ssut_id,
                Store_id = history.Store_id,
                Start_emp_id = history.Start_emp_id,
                Start_time = history.Start_time,
                End_time = history.End_time,
            })
            .FirstOrDefaultAsync();
            return latestInventory;
        }
        catch {  return null; }
    }

    public async Task<Result<InventoryHistory>>  StartNewInventoryAsync(StartNewInventoryHistoryDto model)
    {
        // end current enventory
        await EndInventoryHistoryAsync(model.LatestHistoryId, model.EmpId).ConfigureAwait(false);

        // Set AllProduct Non Inventoried
        await SetAllProductNonInventoriedAsync(model.StoreId, model.EmpId).ConfigureAwait(false);

        // start new enventory
        return await AddInventoryHistoryAsync(model.EmpId, model.StoreId).ConfigureAwait(false);
    }

    internal async Task<Result<InventoryHistory>> AddInventoryHistoryAsync(int empId, int storeId)
    {
        InventoryHistory inventoryHistory = new()
        {
            Store_id = storeId,
            Start_emp_id = empId,
            Start_time = DateTime.Now,
        };

        await context.InventoryHistory.AddAsync(inventoryHistory);
        var rowsAffected = await context.SaveChangesAsync();

        if (rowsAffected > 0)
            return Result<InventoryHistory>.Success(inventoryHistory);
        return Result<InventoryHistory>.Failure();
    }

    internal async Task<bool> EndInventoryHistoryAsync(int id, int empId)
    {
        var currentDate = DateTime.Now;

        var query = await context.InventoryHistory
            .Where(p => p.Ssut_id == id)
            .ExecuteUpdateAsync(en => en
                .SetProperty(p => p.End_emp_id, empId)
                .SetProperty(p => p.End_time, DateTime.Now));
        return (query > 0);
    }

    internal async Task<bool> SetAllProductNonInventoriedAsync(int storeId, int empId)
    {
        var currentDate = DateTime.Now;
        var query = await context.Product_Amount
            .Where(b => b.Store_id == storeId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.Product_update, "0")
            .SetProperty(b => b.Product_update_date, currentDate)
            .SetProperty(b => b.Update_date, currentDate)
            .SetProperty(b => b.Update_uid, empId.ToString()));
        return (query > 0);
    }

    public async Task<SystemVersions?> GetCurrentSystemVersion()
    {
        try
        {
            var latestInventory = await context.Versions.OrderBy(x=>x.Ver_id).LastOrDefaultAsync();
            return latestInventory;
        }
        catch (Exception ex)
        {
            //Console.WriteLine( ex.Message);
        }
        return null;
    }

}
