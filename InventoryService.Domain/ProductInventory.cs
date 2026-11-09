namespace InventoryService.Domain;

public class ProductInventory
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public int AvailableQuantity { get; set; }
}
