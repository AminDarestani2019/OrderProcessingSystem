namespace InventoryService.Domain;

public class ProcessedOrder
{
    public Guid OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public bool WasReserved { get; set; }

    public DateTime ProcessedAt { get; set; }
}
