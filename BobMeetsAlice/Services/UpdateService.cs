namespace BobMeetsAlice.Services
{
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
}
