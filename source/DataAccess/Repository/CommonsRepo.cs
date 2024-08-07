using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Contexts;
using DataAccess.DomainModel;

namespace DataAccess.Repository;

public class CommonsRepo
{
    private readonly AppDb context;

    public CommonsRepo()
    {
        context = new();
    }

    public async Task<List<Stores>> GetAllStores()
    {
        return await context.Stores.ToListAsync();
    }

    public async Task<InventoryHistory?> GetLatestInventoryHistoryAsync(int sotreId)
    {
        var latestInventory = await context.InventoryHistory // أحدث جرد
            .Where(s => s.Start_time != null) // Exclude null dates
            .Where(s => s.Store_id == sotreId)
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

    public async Task<Result<InventoryHistory>>  StartNewInventoryAsync(int latestHistoryId, int empId, int storeId)
    {
        // end current enventory
        await EndInventoryHistoryAsync(latestHistoryId, empId).ConfigureAwait(false);

        // Set AllProduct Non Inventoried
        await SetAllProductNonInventoriedAsync(storeId).ConfigureAwait(false);

        // start new enventory
        return await AddInventoryHistoryAsync(empId,storeId ).ConfigureAwait(false);
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

    internal async Task<bool> SetAllProductNonInventoriedAsync(int storeId)
    {
        var currentDate = DateTime.Now;
        var query = await context.Product_Amount
            .Where(b => b.store_id == storeId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(b => b.Product_update, "0")
            .SetProperty(b => b.Product_update_date, currentDate)
            .SetProperty(b => b.update_date, currentDate)
            .SetProperty(b => b.update_uid, Helper.Constants.UserId));
        return (query > 0);
    }
}
