using DataAccess.Helper;

namespace DataAccess.DomainModel.QueryParams;

public class FilterUsersQParam
{
    public bool? Status { get; set; }
    public bool? EmailConfirmed { get; set; }
    public UsersOrderBy OrderBy { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
