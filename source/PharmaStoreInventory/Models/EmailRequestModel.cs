namespace PharmaStoreInventory.Models;
public class EmailRequestModel
{
    public string UserFullName { get; set; }
    public string Recipient { get; set; }
    public string? Language { get; set; }
    public string? VerificationCode { get; set; }

    public string? Subject { get; set; }
    public string? Body { get; set; }
    public bool IsHtml { get; set; } = true;
}
