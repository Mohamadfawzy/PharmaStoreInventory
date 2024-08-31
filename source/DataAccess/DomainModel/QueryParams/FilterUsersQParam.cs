namespace DataAccess.DomainModel.QueryParams;

public class FilterUsersQParam
{
    public bool? IsActive { get; set; } = false;
    public bool? EmailConfirmed { get; set; } = null;
    public UsersOrderBy OrderBy { get; set; }= UsersOrderBy.CreateOnDescending;
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 30;
}
