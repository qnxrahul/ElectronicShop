namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class AddInventoryItemInput
{
    public string ProductName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string ProductCode { get; set; } = string.Empty;

    public string Hsn { get; set; } = string.Empty;

    public decimal PurchasePrice { get; set; }

    public decimal SalesPrice { get; set; }

    public int OpeningStock { get; set; }

    public int ReorderLevel { get; set; }
}
