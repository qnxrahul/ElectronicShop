namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class BillingInvoiceRow
{
    public int SNo { get; set; }

    public string InvoiceNumber { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int ItemCount { get; set; }

    public decimal TotalAmount { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string CreatedDate { get; set; } = string.Empty;

    public string PaymentType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string InvoiceFlowType { get; set; } = "Invoice";
}
