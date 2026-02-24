namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class BankAccountCard
{
    public string AccountName { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string IconGlyph { get; set; } = "\uE8C7";
}

public sealed class BankTransactionRow
{
    public string TransactionCode { get; set; } = string.Empty;

    public string TransactionDate { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string AccountName { get; set; } = string.Empty;

    public string TransactionType { get; set; } = string.Empty;

    public decimal Amount { get; set; }
}

public sealed class AddBankTransactionInput
{
    public string TransactionType { get; set; } = "Credit";

    public string TransactionDate { get; set; } = string.Empty;

    public string AccountName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string Description { get; set; } = string.Empty;
}
