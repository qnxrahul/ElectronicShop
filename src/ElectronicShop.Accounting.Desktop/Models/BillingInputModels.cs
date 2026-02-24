namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class InvoiceEditorLine
{
    public int RowNumber { get; set; }

    public string Code { get; set; } = string.Empty;

    public string ItemName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Rate { get; set; }

    public decimal Amount { get; set; }
}

public sealed class AddBillingInvoiceInput
{
    public string CustomerName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string PaymentType { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string InvoiceFlowType { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public int ItemCount { get; set; }
}
