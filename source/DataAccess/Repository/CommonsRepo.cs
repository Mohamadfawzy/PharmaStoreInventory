using Microsoft.EntityFrameworkCore;
using DataAccess.Dtos;
using DataAccess.Entities;
using DataAccess.Contexts;

namespace DataAccess.Repository;

public class CommonsRepo
{
    private readonly AppDb context;

    public CommonsRepo(AppDb _context)
    {
        context = _context;
    }

    public Task<List<Stores>> GetAll()
    {
        return context.Stores.ToListAsync();
    }

}
