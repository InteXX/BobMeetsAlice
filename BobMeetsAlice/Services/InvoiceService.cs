using BobMeetsAlice.Models;

namespace BobMeetsAlice.Services
{
  public class InvoiceService
  {
    private readonly Dictionary<int, InvoiceModel> _invoices = [];

    // Method to get an invoice by ID (returns the existing one if it exists)
    public InvoiceModel GetInvoiceById(int invoiceId)
    {
      if (this._invoices.TryGetValue(invoiceId, out InvoiceModel? existingInvoice))
      {
        return existingInvoice; // Return the existing invoice if found
      }

      // If the invoice doesn't exist, create and add it (or handle as needed)
      InvoiceModel newInvoice = new() { Id = invoiceId, Amount = 100, Balance = 100, Status = "New" };
      this._invoices[invoiceId] = newInvoice; // Add the new invoice to the collection
      return newInvoice;
    }
  }
}
