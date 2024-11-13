namespace PharmaStoreInventory.Models;
public class EmailVerificationParameters(Guid eVCID, string code)
{
    public Guid EVCID { get; set; } = eVCID;
    public string? Email { get; set; } 
    public string Code { get; set; } = code;
}