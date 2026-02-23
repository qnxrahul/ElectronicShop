using Dapper;
using ElectronicShop.Accounting.Desktop.Models;
using Microsoft.Data.Sqlite;

namespace ElectronicShop.Accounting.Desktop.Data;

public sealed class AccountingRepository
{
    private readonly string _connectionString;

    public AccountingRepository(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
    }

    public IReadOnlyList<DashboardSummaryCard> GetDashboardSummaryCards()
    {
        const string sql =
            """
            SELECT Title, Amount, ChangePercent, AccentColor, IconGlyph
            FROM DashboardSummaryCards
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<DashboardSummaryCard>(sql).ToList();
    }

    public IReadOnlyList<MonthlySalesPoint> GetMonthlySalesTrend()
    {
        const string sql =
            """
            SELECT Month, Amount, 0 AS BarHeight
            FROM MonthlySalesTrend
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<MonthlySalesPoint>(sql).ToList();
    }

    public IReadOnlyList<TopSellingItem> GetTopSellingItems()
    {
        const string sql =
            """
            SELECT Rank, ItemName, UnitsSold, Revenue
            FROM TopSellingItems
            ORDER BY Rank;
            """;

        using var connection = CreateConnection();
        return connection.Query<TopSellingItem>(sql).ToList();
    }

    public IReadOnlyList<RecentInvoice> GetRecentInvoices()
    {
        const string sql =
            """
            SELECT InvoiceNumber, CustomerName, InvoiceDate, Amount, Status, PaymentType
            FROM RecentInvoices
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<RecentInvoice>(sql).ToList();
    }

    public IReadOnlyList<BranchRevenueItem> GetBranchRevenueItems()
    {
        const string sql =
            """
            SELECT BranchName, Revenue, ChangePercent, AccentColor
            FROM BranchRevenue
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<BranchRevenueItem>(sql).ToList();
    }

    public IReadOnlyList<BillingInvoiceRow> GetBillingInvoices()
    {
        const string sql =
            """
            SELECT SNo, InvoiceNumber, CustomerName, PhoneNumber, ItemCount, TotalAmount, CreatedBy, CreatedDate, PaymentType, Status
            FROM BillingInvoices
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<BillingInvoiceRow>(sql).ToList();
    }

    public IReadOnlyList<InventoryOverviewCard> GetInventoryOverviewCards()
    {
        const string sql =
            """
            SELECT Title, DisplayValue, AccentColor, IconGlyph
            FROM InventoryOverviewCards
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<InventoryOverviewCard>(sql).ToList();
    }

    public IReadOnlyList<InventoryItemRow> GetInventoryItems()
    {
        const string sql =
            """
            SELECT SNo, Sku, ProductName, Category, Hsn, PurchasePrice, SellingPrice, Stock, Status
            FROM InventoryItems
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<InventoryItemRow>(sql).ToList();
    }

    public IReadOnlyList<BankAccountCard> GetBankAccounts()
    {
        const string sql =
            """
            SELECT AccountName, Balance, IconGlyph
            FROM BankAccounts
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<BankAccountCard>(sql).ToList();
    }

    public IReadOnlyList<BankTransactionRow> GetBankTransactions()
    {
        const string sql =
            """
            SELECT TransactionCode, TransactionDate, Description, AccountName, TransactionType, Amount
            FROM BankTransactions
            ORDER BY Id DESC;
            """;

        using var connection = CreateConnection();
        return connection.Query<BankTransactionRow>(sql).ToList();
    }

    private SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
