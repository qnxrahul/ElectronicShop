namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class DashboardSummaryCard
{
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public decimal ChangePercent { get; set; }

    public string AccentColor { get; set; } = "#E9EEF8";

    public string IconGlyph { get; set; } = "\uE8C7";
}

public sealed class MonthlySalesPoint
{
    public string Month { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public double BarHeight { get; set; }
}

public sealed class TopSellingItem
{
    public int Rank { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public int UnitsSold { get; set; }

    public decimal Revenue { get; set; }
}

public sealed class RecentInvoice
{
    public string InvoiceNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string InvoiceDate { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Status { get; set; } = string.Empty;

    public string PaymentType { get; set; } = string.Empty;
}

public sealed class BranchRevenueItem
{
    public string BranchName { get; set; } = string.Empty;

    public decimal Revenue { get; set; }

    public decimal ChangePercent { get; set; }

    public string AccentColor { get; set; } = "#EAF0FF";
}
