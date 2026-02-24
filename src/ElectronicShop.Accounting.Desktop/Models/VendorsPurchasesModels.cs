namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class VendorRow
{
    public int SNo { get; set; }

    public string VendorName { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public decimal OutstandingBalance { get; set; }

    public string Status { get; set; } = string.Empty;
}

public sealed class PurchaseInvoiceRow
{
    public int SNo { get; set; }

    public string PONumber { get; set; } = string.Empty;

    public string VendorName { get; set; } = string.Empty;

    public int TotalProduct { get; set; }

    public int TotalQuantity { get; set; }

    public decimal TotalAmount { get; set; }

    public string OrderDate { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

public sealed class PurchaseReturnRow
{
    public int SNo { get; set; }

    public string PONumber { get; set; } = string.Empty;

    public string VendorName { get; set; } = string.Empty;

    public int PurchasedQuantity { get; set; }

    public int ReturnQuantity { get; set; }

    public decimal ReturnTotal { get; set; }

    public string OrderDate { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

public sealed class VendorPayBillRow
{
    public int SNo { get; set; }

    public string PONumber { get; set; } = string.Empty;

    public string VendorName { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = string.Empty;

    public decimal PaidAmount { get; set; }

    public decimal PendingAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string DueDate { get; set; } = string.Empty;

    public string LastPaymentDate { get; set; } = string.Empty;
}

public sealed class AddVendorInput
{
    public string VendorName { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string AccountName { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;
}

public sealed class AddPurchaseInvoiceInput
{
    public string VendorName { get; set; } = string.Empty;

    public string PurchaseDate { get; set; } = string.Empty;

    public string PurchaseStatus { get; set; } = "Pending";

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string PaymentMode { get; set; } = string.Empty;
}

public sealed class AddPurchaseReturnInput
{
    public string VendorName { get; set; } = string.Empty;

    public string InvoiceNumber { get; set; } = string.Empty;

    public string ReturnDate { get; set; } = string.Empty;

    public int ReturnQuantity { get; set; }

    public decimal UnitPrice { get; set; }
}

public sealed class AddVendorPaymentInput
{
    public string VendorName { get; set; } = string.Empty;

    public string InvoiceNumber { get; set; } = string.Empty;

    public string PaymentDate { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string PaymentMode { get; set; } = string.Empty;
}
