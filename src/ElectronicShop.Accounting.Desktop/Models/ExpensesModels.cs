namespace ElectronicShop.Accounting.Desktop.Models;

public sealed class ExpenseSummaryCard
{
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string AccentColor { get; set; } = "#D8E9FB";

    public string IconGlyph { get; set; } = "\uEAFD";
}

public sealed class ExpenseRow
{
    public string ExpenseId { get; set; } = string.Empty;

    public string ExpenseDate { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }
}

public sealed class ExpenseCategoryBreakdown
{
    public string Category { get; set; } = string.Empty;

    public int Percentage { get; set; }

    public double BarWidth { get; set; }
}

public sealed class AddExpenseInput
{
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string ExpenseDate { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string PaidFrom { get; set; } = string.Empty;
}
