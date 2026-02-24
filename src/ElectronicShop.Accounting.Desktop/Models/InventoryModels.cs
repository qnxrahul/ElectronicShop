namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class InventoryOverviewCard
{
    public string Title { get; set; } = string.Empty;

    public string DisplayValue { get; set; } = string.Empty;

    public string AccentColor { get; set; } = "#EAF0FF";

    public string IconGlyph { get; set; } = "\uE7BF";
}

public sealed class InventoryItemRow
{
    public int SNo { get; set; }

    public string Sku { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Hsn { get; set; } = string.Empty;

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public int Stock { get; set; }

    public string Status { get; set; } = string.Empty;
}
