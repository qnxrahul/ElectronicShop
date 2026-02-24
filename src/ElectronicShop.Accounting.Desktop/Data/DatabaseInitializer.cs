using System.Data;
using System.IO;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ElectronicShop.Accounting.Desktop.Data;

public sealed class DatabaseInitializer
{
    private readonly string _databasePath;

    public DatabaseInitializer(string databasePath)
    {
        _databasePath = databasePath;
    }

    public void Initialize()
    {
        var folder = Path.GetDirectoryName(_databasePath);
        if (!string.IsNullOrWhiteSpace(folder))
        {
            Directory.CreateDirectory(folder);
        }

        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        connection.Execute("PRAGMA foreign_keys = ON;");
        connection.Execute("PRAGMA journal_mode = WAL;");

        CreateSchema(connection);
        SeedData(connection);
    }

    private static void CreateSchema(IDbConnection connection)
    {
        connection.Execute(
            """
            CREATE TABLE IF NOT EXISTS DashboardSummaryCards (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Amount REAL NOT NULL,
                ChangePercent REAL NOT NULL,
                AccentColor TEXT NOT NULL,
                IconGlyph TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS MonthlySalesTrend (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Month TEXT NOT NULL,
                Amount REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS TopSellingItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Rank INTEGER NOT NULL,
                ItemName TEXT NOT NULL,
                UnitsSold INTEGER NOT NULL,
                Revenue REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS RecentInvoices (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                InvoiceNumber TEXT NOT NULL,
                CustomerName TEXT NOT NULL,
                InvoiceDate TEXT NOT NULL,
                Amount REAL NOT NULL,
                Status TEXT NOT NULL,
                PaymentType TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BranchRevenue (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BranchName TEXT NOT NULL,
                Revenue REAL NOT NULL,
                ChangePercent REAL NOT NULL,
                AccentColor TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BillingInvoices (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SNo INTEGER NOT NULL,
                InvoiceNumber TEXT NOT NULL,
                CustomerName TEXT NOT NULL,
                PhoneNumber TEXT NOT NULL,
                ItemCount INTEGER NOT NULL,
                TotalAmount REAL NOT NULL,
                CreatedBy TEXT NOT NULL,
                CreatedDate TEXT NOT NULL,
                PaymentType TEXT NOT NULL,
                Status TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS InventoryOverviewCards (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                DisplayValue TEXT NOT NULL,
                AccentColor TEXT NOT NULL,
                IconGlyph TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS InventoryItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SNo INTEGER NOT NULL,
                Sku TEXT NOT NULL,
                ProductName TEXT NOT NULL,
                Category TEXT NOT NULL,
                Hsn TEXT NOT NULL,
                PurchasePrice REAL NOT NULL,
                SellingPrice REAL NOT NULL,
                Stock INTEGER NOT NULL,
                Status TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BankAccounts (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AccountName TEXT NOT NULL,
                Balance REAL NOT NULL,
                IconGlyph TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BankTransactions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TransactionCode TEXT NOT NULL,
                TransactionDate TEXT NOT NULL,
                Description TEXT NOT NULL,
                AccountName TEXT NOT NULL,
                TransactionType TEXT NOT NULL,
                Amount REAL NOT NULL
            );
            """);
    }

    private static void SeedData(IDbConnection connection)
    {
        SeedDashboard(connection);
        SeedBilling(connection);
        SeedInventory(connection);
        SeedBanking(connection);
    }

    private static void SeedDashboard(IDbConnection connection)
    {
        if (TableHasRows(connection, "DashboardSummaryCards"))
        {
            return;
        }

        connection.Execute(
            """
            INSERT INTO DashboardSummaryCards (Title, Amount, ChangePercent, AccentColor, IconGlyph)
            VALUES (@Title, @Amount, @ChangePercent, @AccentColor, @IconGlyph);
            """,
            new[]
            {
                new { Title = "Total Sale", Amount = 74500m, ChangePercent = 21m, AccentColor = "#D8E9FB", IconGlyph = "\uE8C7" },
                new { Title = "Total Profit", Amount = 40352m, ChangePercent = 17m, AccentColor = "#E5DCF9", IconGlyph = "\uE9D2" },
                new { Title = "Pending Payment", Amount = 14515m, ChangePercent = 17m, AccentColor = "#D9F2DE", IconGlyph = "\uE8AB" },
                new { Title = "Cash Balance", Amount = 74500m, ChangePercent = 21m, AccentColor = "#EFE9CD", IconGlyph = "\uEAFD" },
                new { Title = "Bank Balance", Amount = 74500m, ChangePercent = 21m, AccentColor = "#DDEBFA", IconGlyph = "\uE825" },
                new { Title = "Low Stock Items", Amount = 14m, ChangePercent = 21m, AccentColor = "#F4DDDF", IconGlyph = "\uEA39" }
            });

        connection.Execute(
            """
            INSERT INTO MonthlySalesTrend (Month, Amount)
            VALUES (@Month, @Amount);
            """,
            new[]
            {
                new { Month = "Jan", Amount = 48m },
                new { Month = "Feb", Amount = 72m },
                new { Month = "Mar", Amount = 14m },
                new { Month = "Apr", Amount = 32m },
                new { Month = "May", Amount = 90m },
                new { Month = "Jun", Amount = 24m },
                new { Month = "Jul", Amount = 49m },
                new { Month = "Aug", Amount = 70m },
                new { Month = "Sep", Amount = 49m },
                new { Month = "Oct", Amount = 49m },
                new { Month = "Nov", Amount = 66m },
                new { Month = "Dec", Amount = 50m }
            });

        connection.Execute(
            """
            INSERT INTO TopSellingItems (Rank, ItemName, UnitsSold, Revenue)
            VALUES (@Rank, @ItemName, @UnitsSold, @Revenue);
            """,
            new[]
            {
                new { Rank = 1, ItemName = "LED Bulb 9W", UnitsSold = 245, Revenue = 35500m },
                new { Rank = 2, ItemName = "LED Bulb 9W", UnitsSold = 245, Revenue = 35500m },
                new { Rank = 3, ItemName = "LED Bulb 9W", UnitsSold = 245, Revenue = 35500m },
                new { Rank = 4, ItemName = "LED Bulb 9W", UnitsSold = 245, Revenue = 35500m },
                new { Rank = 5, ItemName = "LED Bulb 9W", UnitsSold = 245, Revenue = 35500m },
                new { Rank = 6, ItemName = "LED Bulb 9W", UnitsSold = 245, Revenue = 35500m },
                new { Rank = 7, ItemName = "LED Bulb 9W", UnitsSold = 245, Revenue = 35500m }
            });

        connection.Execute(
            """
            INSERT INTO RecentInvoices (InvoiceNumber, CustomerName, InvoiceDate, Amount, Status, PaymentType)
            VALUES (@InvoiceNumber, @CustomerName, @InvoiceDate, @Amount, @Status, @PaymentType);
            """,
            new[]
            {
                new { InvoiceNumber = "INV-2024-0156", CustomerName = "Sharma Electricals", InvoiceDate = "2024-01-15", Amount = 12450m, Status = "Paid", PaymentType = "UPI" },
                new { InvoiceNumber = "INV-2024-0157", CustomerName = "Sharma Electricals", InvoiceDate = "2024-01-15", Amount = 12450m, Status = "Paid", PaymentType = "UPI" },
                new { InvoiceNumber = "INV-2024-0158", CustomerName = "Sharma Electricals", InvoiceDate = "2024-01-15", Amount = 12450m, Status = "Paid", PaymentType = "UPI" },
                new { InvoiceNumber = "INV-2024-0159", CustomerName = "Sharma Electricals", InvoiceDate = "2024-01-15", Amount = 12450m, Status = "Paid", PaymentType = "UPI" },
                new { InvoiceNumber = "INV-2024-0160", CustomerName = "Sharma Electricals", InvoiceDate = "2024-01-15", Amount = 12450m, Status = "Paid", PaymentType = "UPI" },
                new { InvoiceNumber = "INV-2024-0161", CustomerName = "Sharma Electricals", InvoiceDate = "2024-01-15", Amount = 12450m, Status = "Paid", PaymentType = "UPI" }
            });

        connection.Execute(
            """
            INSERT INTO BranchRevenue (BranchName, Revenue, ChangePercent, AccentColor)
            VALUES (@BranchName, @Revenue, @ChangePercent, @AccentColor);
            """,
            new[]
            {
                new { BranchName = "Branch 01", Revenue = 35052m, ChangePercent = 21m, AccentColor = "#DDEBFA" },
                new { BranchName = "Branch 02", Revenue = 35052m, ChangePercent = 21m, AccentColor = "#EAE2FA" },
                new { BranchName = "Branch 03", Revenue = 35052m, ChangePercent = 21m, AccentColor = "#F3EFCF" }
            });
    }

    private static void SeedBilling(IDbConnection connection)
    {
        if (TableHasRows(connection, "BillingInvoices"))
        {
            return;
        }

        connection.Execute(
            """
            INSERT INTO BillingInvoices
            (SNo, InvoiceNumber, CustomerName, PhoneNumber, ItemCount, TotalAmount, CreatedBy, CreatedDate, PaymentType, Status)
            VALUES
            (@SNo, @InvoiceNumber, @CustomerName, @PhoneNumber, @ItemCount, @TotalAmount, @CreatedBy, @CreatedDate, @PaymentType, @Status);
            """,
            new[]
            {
                new { SNo = 1, InvoiceNumber = "ETS-26001", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" },
                new { SNo = 2, InvoiceNumber = "ETS-26002", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" },
                new { SNo = 3, InvoiceNumber = "ETS-26003", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" },
                new { SNo = 4, InvoiceNumber = "ETS-26004", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" },
                new { SNo = 5, InvoiceNumber = "ETS-26005", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" },
                new { SNo = 6, InvoiceNumber = "ETS-26006", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" },
                new { SNo = 7, InvoiceNumber = "ETS-26007", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" },
                new { SNo = 8, InvoiceNumber = "ETS-26008", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft" }
            });
    }

    private static void SeedInventory(IDbConnection connection)
    {
        if (!TableHasRows(connection, "InventoryOverviewCards"))
        {
            connection.Execute(
                """
                INSERT INTO InventoryOverviewCards (Title, DisplayValue, AccentColor, IconGlyph)
                VALUES (@Title, @DisplayValue, @AccentColor, @IconGlyph);
                """,
                new[]
                {
                    new { Title = "Total Items", DisplayValue = "12", AccentColor = "#D8E9FB", IconGlyph = "\uE7BF" },
                    new { Title = "Low Stock Alert", DisplayValue = "3", AccentColor = "#F2DEE0", IconGlyph = "\uE814" },
                    new { Title = "Stock Value(Cost)", DisplayValue = "$2,451", AccentColor = "#F3EFCF", IconGlyph = "\uEAFD" },
                    new { Title = "Stock Value (Selling)", DisplayValue = "$2,451", AccentColor = "#D9F2DE", IconGlyph = "\uE9D2" }
                });
        }

        if (TableHasRows(connection, "InventoryItems"))
        {
            return;
        }

        connection.Execute(
            """
            INSERT INTO InventoryItems
            (SNo, Sku, ProductName, Category, Hsn, PurchasePrice, SellingPrice, Stock, Status)
            VALUES
            (@SNo, @Sku, @ProductName, @Category, @Hsn, @PurchasePrice, @SellingPrice, @Stock, @Status);
            """,
            new[]
            {
                new { SNo = 1, Sku = "ITM-001", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good" },
                new { SNo = 2, Sku = "ITM-002", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 40, Status = "Low" },
                new { SNo = 3, Sku = "ITM-003", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 5, Status = "Critical" },
                new { SNo = 4, Sku = "ITM-004", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good" },
                new { SNo = 5, Sku = "ITM-005", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good" },
                new { SNo = 6, Sku = "ITM-006", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good" },
                new { SNo = 7, Sku = "ITM-007", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good" },
                new { SNo = 8, Sku = "ITM-008", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good" }
            });
    }

    private static void SeedBanking(IDbConnection connection)
    {
        if (!TableHasRows(connection, "BankAccounts"))
        {
            connection.Execute(
                """
                INSERT INTO BankAccounts (AccountName, Balance, IconGlyph)
                VALUES (@AccountName, @Balance, @IconGlyph);
                """,
                new[]
                {
                    new { AccountName = "Cash Account", Balance = 99000m, IconGlyph = "\uE8C7" },
                    new { AccountName = "HDFC Bank - Current", Balance = 99000m, IconGlyph = "\uE825" },
                    new { AccountName = "ICICI Bank - Savings", Balance = 99000m, IconGlyph = "\uE825" }
                });
        }

        if (TableHasRows(connection, "BankTransactions"))
        {
            return;
        }

        connection.Execute(
            """
            INSERT INTO BankTransactions
            (TransactionCode, TransactionDate, Description, AccountName, TransactionType, Amount)
            VALUES
            (@TransactionCode, @TransactionDate, @Description, @AccountName, @TransactionType, @Amount);
            """,
            new[]
            {
                new { TransactionCode = "ETS-26001", TransactionDate = "22/10/2026", Description = "Payment from Sharma Electricals", AccountName = "HDFC Bank", TransactionType = "Credit", Amount = 4500m },
                new { TransactionCode = "ETS-26002", TransactionDate = "22/10/2026", Description = "Warehouse utility bill", AccountName = "Cash Account", TransactionType = "Debit", Amount = 850m },
                new { TransactionCode = "ETS-26003", TransactionDate = "22/10/2026", Description = "Vendor payment", AccountName = "ICICI Bank", TransactionType = "Debit", Amount = 2900m },
                new { TransactionCode = "ETS-26004", TransactionDate = "23/10/2026", Description = "Customer received", AccountName = "HDFC Bank", TransactionType = "Credit", Amount = 5200m }
            });
    }

    private static bool TableHasRows(IDbConnection connection, string tableName)
    {
        var count = connection.ExecuteScalar<long>($"SELECT COUNT(1) FROM {tableName};");
        return count > 0;
    }
}
