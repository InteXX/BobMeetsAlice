namespace BobMeetsAlice.Models
{
  public class InvoiceModel
  {
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public decimal Payment { get; set; }
    public required string Status { get; set; }
  }
}
