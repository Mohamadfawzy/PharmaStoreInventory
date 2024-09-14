using DataAccess.Entities;

namespace DataAccess.Dtos;

public class BrancheDto
{
    public Guid Id { get; set; }
    public string? BrachName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Telephone { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Port { get; set; } = string.Empty;
    public DateTime? CreateOn { get; set; }
    public int UserId { get; set; }

    public static implicit operator Branche(BrancheDto model)
    {
        return new Branche
        {
            Id = model.Id,
            BrachName = model.BrachName,
            Username = model.Username,
            Password = model.Password,
            Telephone = model.Telephone,
            IpAddress = model.IpAddress,
            Port = model.Port,
            //CreateOn = model.CreateOn,
            UserId = model.UserId,
           
        };
    }
    
    public static implicit operator BrancheDto(Branche model)
    {
        return new BrancheDto
        {
            Id = model.Id,
            BrachName = model.BrachName,
            Username = model.Username,
            Password = model.Password,
            Telephone = model.Telephone,
            IpAddress = model.IpAddress,
            Port = model.Port,
            CreateOn = model.CreateOn,
            UserId = model.UserId,
        };
    }
}
