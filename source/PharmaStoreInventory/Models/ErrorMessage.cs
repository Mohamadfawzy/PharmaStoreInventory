namespace PharmaStoreInventory.Models
{
    public class ErrorMessage(string title="Error", string body="")
    {
        public string Title { get; set; } = title;
        public string Body { get; set; } = body;
    }
}
