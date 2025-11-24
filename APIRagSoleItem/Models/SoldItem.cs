namespace APIRagSoleItem.Models;
public record SoldItem(
    DateTime SaleDate,
    int ProductId,
    string ProductName,
    string? Category,
    int Quantity,
    decimal UnitPrice,
    int? CustomerId,
    string? Note
);

public class SoldItemDto
{
    public int Id { get; set; }
    public string Product { get; set; } = string.Empty;
}