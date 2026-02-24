namespace ElectronicShop.Accounting.Desktop.Models;

public enum ReportTab
{
    SalesReport,
    PurchaseReport,
    StockReport,
    CustomerOutstanding,
    VendorOutstanding,
    ProfitAndLoss,
    BalanceSheet
}

public sealed class ReportMetricCard
{
    public string Title { get; set; } = string.Empty;

    public string DisplayValue { get; set; } = string.Empty;

    public string TrendText { get; set; } = string.Empty;

    public string AccentColor { get; set; } = "#D8E9FB";
}

public sealed class SalesReportRow
{
    public string Date { get; set; } = string.Empty;

    public string BillNo { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string ItemName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Rate { get; set; }

    public decimal Amount { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;

    public string PaymentMode { get; set; } = string.Empty;

    public decimal Profit { get; set; }
}

public sealed class PurchaseReportRow
{
    public string Date { get; set; } = string.Empty;

    public string BillNo { get; set; } = string.Empty;

    public string VendorName { get; set; } = string.Empty;

    public string ItemName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;
}

public sealed class StockReportRow
{
    public string ItemName { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public int OpeningStock { get; set; }

    public int PurchaseQuantity { get; set; }

    public int SalesQuantity { get; set; }

    public int CurrentStock { get; set; }

    public decimal PurchaseValue { get; set; }

    public decimal SalesValue { get; set; }

    public decimal ProfitMargin { get; set; }
}

public sealed class CustomerOutstandingRow
{
    public string CustomerName { get; set; } = string.Empty;

    public string InvoiceNo { get; set; } = string.Empty;

    public string InvoiceDate { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal Balance { get; set; }
}

public sealed class VendorOutstandingRow
{
    public string VendorName { get; set; } = string.Empty;

    public string BillNo { get; set; } = string.Empty;

    public string BillDate { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal Balance { get; set; }
}

public sealed class ProfitLossRow
{
    public int Number { get; set; }

    public string Product { get; set; } = string.Empty;

    public int UnitsSold { get; set; }

    public decimal CostPrice { get; set; }

    public decimal SalesPrice { get; set; }

    public decimal TotalProfit { get; set; }

    public decimal MarginPercent { get; set; }
}

public sealed class BalanceSheetEntry
{
    public string Section { get; set; } = string.Empty;

    public string AccountName { get; set; } = string.Empty;

    public decimal Amount { get; set; }
}

public sealed class BalanceSheetTotals
{
    public decimal TotalAssets { get; set; }

    public decimal TotalLiabilitiesAndEquity { get; set; }
}
