namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class AddBankTransactionInput
{
    public string TransactionType { get; set; } = string.Empty;

    public string TransactionDate { get; set; } = string.Empty;

    public string AccountName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;
}
