# Invoice Payment System

This sample Blazor Server application demonstrates a simple invoice management system where
payments applied to invoices are reflected in real time for all subscribers.

## Features

- **Invoice Details**: Displays invoice information such as `Amount` and `Status`.
- **Payment Input**: Allows users to input and apply a payment amount to an invoice.
- **Real-time Invoice Updates**: Uses a custom service to notify all subscribers of changes,
ensuring the UI is refreshed for all users with the latest data.
- **Invoice Management**: Supports adding, updating, and tracking invoice details.

## How It Works

1. **Loading an Invoice**  
   - If the invoice is not already loaded, it is fetched from `InvoiceService`.
   - If it is already loaded, its values are updated rather than replaced.

2. **Making a Payment**  
   - The user enters a payment amount.
   - The invoice status changes to `"Paid"`, and the amount is reduced accordingly.
   - A notification service updates all subscribers.

3. **Real-time Updates**  
   - When an invoice is updated, all subscribed components receive a notification.
   - The UI is refreshed to display the latest values.

---

## Core Concepts

### `InvoiceModel`
The `InvoiceModel` class represents an invoice and holds important information about the invoice:

- **Id**: A unique identifier for each invoice.
- **Amount**: The original amount due for the invoice.
- **Balance**: The current amount due for the invoice.
- **Payment**: The payment amount for the invoice.
- **Status**: The current status of the invoice, such as "Pending", "Paid", etc.

Example:

```csharp
public class InvoiceModel
{
  public int Id { get; set; }
  public decimal Amount { get; set; }
  public decimal Balance { get; set; }
  public decimal Payment { get; set; }
  public required string Status { get; set; };
}
```

### `InvoiceService`

This service is responsible for fetching and managing invoice data, either from a database or
another external source. In this example, the `GetInvoiceById` method simulates retrieving
an invoice by `Id`.

Example:

```csharp
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
```

### `UpdateService`

This service is responsible for notifying subscribers when an invoice has been
updated. It uses a list of callback actions to communicate the changes.

- **Subscribe**: Components subscribe to receive updates when the invoice changes.
- **Unsubscribe**: Components can unsubscribe when they no longer need updates.
- **NotifyInvoiceUpdated**: When an invoice is updated, this method is called to
notify all subscribers to refresh their data.

Example:

```csharp
public class UpdateService
{
  private readonly List<Action<int>> _subscribers = [];

  public void Subscribe(Action<int> callback)
  {
    this._subscribers.Add(callback);
  }

  public void Unsubscribe(Action<int> callback)
  {
    this._subscribers.Remove(callback);
  }

  public void NotifyInvoiceUpdated(int invoiceId)
  {
    foreach (Action<int> callback in this._subscribers)
    {
      callback(invoiceId); // Notify subscribers that the invoice has been updated
    }
  }
}
```

### `Invoice.razor` (Main Component)

This Razor component displays the invoice details and allows the user to input a payment amount.

- **LoadInvoice**: Loads the current invoice details from the `InvoiceService` based on the invoice
`Id`. If the invoice is already loaded, it updates its properties (such as `Amount` and `Status`)
instead of reloading the entire object.

Example:
```csharp
private void LoadInvoice()
{
  if (invoice == null)
  {
    invoice = InvoiceService.GetInvoiceById(InvoiceId);
  }
  else
  {
    var updatedInvoice = InvoiceService.GetInvoiceById(InvoiceId);

    if (updatedInvoice != null)
    {
      invoice.Payment = updatedInvoice.Payment;
      invoice.Balance = updatedInvoice.Balance;
      invoice.Status = updatedInvoice.Status;
    }
  }

  InvokeAsync(StateHasChanged); // Refresh the UI
}
```

- **MakePayment**: When the user enters a payment amount and clicks the "Make Payment" button,
the status of the invoice is updated and the invoice amount is adjusted accordingly. Afterward,
the `InvoiceUpdateService.NotifyInvoiceUpdated()` method is triggered to inform other components
of the update.

Example:
```csharp
private void MakePayment()
{
  if (invoice is null) return;

  invoice.Balance -= paymentAmount;
  invoice.Payment = paymentAmount;
  invoice.Status = invoice.Balance == 0 ? "Paid" : (invoice.Balance < 0 ? "Overpaid" : "Pending");

  UpdateService.NotifyInvoiceUpdated(InvoiceId);
}
```

### Real-Time Updates with `StateHasChanged`

When the invoice is updated, the `StateHasChanged()` method is called to re-render the component,
ensuring the UI reflects the new values. This method triggers a refresh only for components that
have subscribed to the update.

The `UpdateService.NotifyInvoiceUpdated()` method ensures that all subscribers to the invoice
update are notified when the invoice is updated, allowing each component to re-render itself
and display the latest data.

## Workflow

1. **Load Invoice**: When the page loads, the `LoadInvoice()` method checks if the invoice is loaded.
If not, it fetches it from the `InvoiceService`. If the invoice already exists, it updates its
properties with the latest values.

2. **Make Payment**: The user enters a payment amount, and when the "Make Payment" button is clicked,
the MakePayment method updates the invoice status and the amount.

3. **Real-Time Updates**: After the invoice is updated, `NotifyInvoiceUpdated()` triggers updates
to all subscribers (other components listening to invoice changes), ensuring real-time
synchronization across users.

## How to Run the Application

1. Clone the repository:
```bash
git clone https://github.com/intexx/BobMeetsAlice.git
```
2. Open the project in Visual Studio or your preferred IDE.
3. Build and run the application.
4. Navigate to the invoice page, duplicate the tab, and the tear it off into a separate window
in order to simulate multiple users.
5. Input the payment amount on any tab and click "Make Payment." You’ll see the invoice details
update in real time for all users.

## Technologies Used

- **Blazor WebApp**: For building interactive web applications.
- **C# & .NET 8**: The application is built with the latest version of .NET 8.
- **SignalR-like Event Notifications**: Using a custom service to notify components of invoice
updates in real time.
- **Razor Components**: To manage the UI and user interactions.

## Additional Notes

- The application ensures that updates to the invoice are handled efficiently without needing
to reload the entire page.
- Real-time synchronization ensures that all users view the latest status and amount of the
invoice.
---
