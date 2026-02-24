using System.Data;
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
        ApplyMigrations(connection);
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
                Status TEXT NOT NULL,
                InvoiceFlowType TEXT NOT NULL DEFAULT 'Invoice'
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
                Status TEXT NOT NULL,
                ReorderLevel INTEGER NOT NULL DEFAULT 10
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

            CREATE TABLE IF NOT EXISTS Vendors (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SNo INTEGER NOT NULL,
                VendorName TEXT NOT NULL,
                CompanyName TEXT NOT NULL,
                PhoneNumber TEXT NOT NULL,
                EmailAddress TEXT NOT NULL,
                OutstandingBalance REAL NOT NULL,
                Status TEXT NOT NULL,
                Address TEXT NOT NULL,
                AccountName TEXT NOT NULL,
                AccountNumber TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS PurchaseInvoices (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SNo INTEGER NOT NULL,
                PONumber TEXT NOT NULL,
                VendorName TEXT NOT NULL,
                TotalProduct INTEGER NOT NULL,
                TotalQuantity INTEGER NOT NULL,
                TotalAmount REAL NOT NULL,
                OrderDate TEXT NOT NULL,
                Status TEXT NOT NULL,
                ProductName TEXT NOT NULL,
                Quantity INTEGER NOT NULL,
                Price REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS PurchaseReturns (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SNo INTEGER NOT NULL,
                PONumber TEXT NOT NULL,
                VendorName TEXT NOT NULL,
                PurchasedQuantity INTEGER NOT NULL,
                ReturnQuantity INTEGER NOT NULL,
                ReturnTotal REAL NOT NULL,
                OrderDate TEXT NOT NULL,
                Status TEXT NOT NULL,
                InvoiceNumber TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS VendorPayments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SNo INTEGER NOT NULL,
                PONumber TEXT NOT NULL,
                VendorName TEXT NOT NULL,
                PaymentStatus TEXT NOT NULL,
                PaidAmount REAL NOT NULL,
                PendingAmount REAL NOT NULL,
                TotalAmount REAL NOT NULL,
                DueDate TEXT NOT NULL,
                LastPaymentDate TEXT NOT NULL,
                PaymentMode TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ExpenseSummaryCards (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Amount REAL NOT NULL,
                AccentColor TEXT NOT NULL,
                IconGlyph TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Expenses (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ExpenseId TEXT NOT NULL,
                ExpenseDate TEXT NOT NULL,
                Category TEXT NOT NULL,
                Description TEXT NOT NULL,
                Amount REAL NOT NULL,
                PaidFrom TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ExpenseCategoryBreakdown (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Category TEXT NOT NULL,
                Percentage INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ReportMetricCards (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ReportType TEXT NOT NULL,
                Title TEXT NOT NULL,
                DisplayValue TEXT NOT NULL,
                TrendText TEXT NOT NULL,
                AccentColor TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS SalesReportRows (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT NOT NULL,
                BillNo TEXT NOT NULL,
                CustomerName TEXT NOT NULL,
                ItemName TEXT NOT NULL,
                Quantity INTEGER NOT NULL,
                Rate REAL NOT NULL,
                Amount REAL NOT NULL,
                PaymentStatus TEXT NOT NULL,
                PaymentMode TEXT NOT NULL,
                Profit REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS PurchaseReportRows (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT NOT NULL,
                BillNo TEXT NOT NULL,
                VendorName TEXT NOT NULL,
                ItemName TEXT NOT NULL,
                Quantity INTEGER NOT NULL,
                PurchasePrice REAL NOT NULL,
                TotalAmount REAL NOT NULL,
                PaymentStatus TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS StockReportRows (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ItemName TEXT NOT NULL,
                Category TEXT NOT NULL,
                OpeningStock INTEGER NOT NULL,
                PurchaseQuantity INTEGER NOT NULL,
                SalesQuantity INTEGER NOT NULL,
                CurrentStock INTEGER NOT NULL,
                PurchaseValue REAL NOT NULL,
                SalesValue REAL NOT NULL,
                ProfitMargin REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS CustomerOutstandingRows (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CustomerName TEXT NOT NULL,
                InvoiceNo TEXT NOT NULL,
                InvoiceDate TEXT NOT NULL,
                TotalAmount REAL NOT NULL,
                PaidAmount REAL NOT NULL,
                Balance REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS VendorOutstandingRows (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                VendorName TEXT NOT NULL,
                BillNo TEXT NOT NULL,
                BillDate TEXT NOT NULL,
                TotalAmount REAL NOT NULL,
                PaidAmount REAL NOT NULL,
                Balance REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS ProfitLossRows (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Number INTEGER NOT NULL,
                Product TEXT NOT NULL,
                UnitsSold INTEGER NOT NULL,
                CostPrice REAL NOT NULL,
                SalesPrice REAL NOT NULL,
                TotalProfit REAL NOT NULL,
                MarginPercent REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BalanceSheetEntries (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                EntryType TEXT NOT NULL,
                Section TEXT NOT NULL,
                AccountName TEXT NOT NULL,
                Amount REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BalanceSheetTotals (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TotalAssets REAL NOT NULL,
                TotalLiabilitiesAndEquity REAL NOT NULL
            );

            CREATE TABLE IF NOT EXISTS CompanyProfileSettings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CompanyName TEXT NOT NULL,
                MobileNumber TEXT NOT NULL,
                EmailAddress TEXT NOT NULL,
                Address TEXT NOT NULL,
                LogoPath TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS LedgerAccounts (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AccountName TEXT NOT NULL,
                AccountType TEXT NOT NULL,
                OpeningBalance REAL NOT NULL,
                CreatedDate TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS AppUsers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserName TEXT NOT NULL,
                EmailAddress TEXT NOT NULL,
                MobileNumber TEXT NOT NULL,
                Role TEXT NOT NULL,
                IsActive INTEGER NOT NULL,
                PasswordHash TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BackupStatus (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CurrentStatus TEXT NOT NULL,
                LastSuccessfulBackup TEXT NOT NULL,
                NextScheduledBackup TEXT NOT NULL,
                AutoBackupEnabled INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS BackupLogs (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Activity TEXT NOT NULL,
                DateAndTime TEXT NOT NULL,
                Size TEXT NOT NULL,
                Status TEXT NOT NULL
            );
            """);
    }

    private static void ApplyMigrations(IDbConnection connection)
    {
        EnsureColumn(connection, "BillingInvoices", "InvoiceFlowType", "TEXT NOT NULL DEFAULT 'Invoice'");
        EnsureColumn(connection, "InventoryItems", "ReorderLevel", "INTEGER NOT NULL DEFAULT 10");
    }

    private static void EnsureColumn(IDbConnection connection, string tableName, string columnName, string definition)
    {
        var columns = connection.Query<TableInfoColumn>($"PRAGMA table_info({tableName});")
            .Select(column => column.Name)
            .ToList();
        if (columns.Contains(columnName, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        connection.Execute($"ALTER TABLE {tableName} ADD COLUMN {columnName} {definition};");
    }

    private sealed class TableInfoColumn
    {
        public string Name { get; set; } = string.Empty;
    }

    private static void SeedData(IDbConnection connection)
    {
        SeedDashboard(connection);
        SeedBilling(connection);
        SeedInventory(connection);
        SeedBanking(connection);
        SeedVendorsAndPurchases(connection);
        SeedExpenses(connection);
        SeedReports(connection);
        SeedSettings(connection);
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
            BackfillBillingFlowData(connection);
            return;
        }

        connection.Execute(
            """
            INSERT INTO BillingInvoices
            (SNo, InvoiceNumber, CustomerName, PhoneNumber, ItemCount, TotalAmount, CreatedBy, CreatedDate, PaymentType, Status, InvoiceFlowType)
            VALUES
            (@SNo, @InvoiceNumber, @CustomerName, @PhoneNumber, @ItemCount, @TotalAmount, @CreatedBy, @CreatedDate, @PaymentType, @Status, @InvoiceFlowType);
            """,
            new[]
            {
                new { SNo = 1, InvoiceNumber = "ETS-26001", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft", InvoiceFlowType = "Invoice" },
                new { SNo = 2, InvoiceNumber = "ETS-26002", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft", InvoiceFlowType = "Invoice" },
                new { SNo = 3, InvoiceNumber = "ETS-26003", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft", InvoiceFlowType = "Invoice" },
                new { SNo = 4, InvoiceNumber = "ETS-26004", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft", InvoiceFlowType = "Invoice" },
                new { SNo = 5, InvoiceNumber = "ETS-26005", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft", InvoiceFlowType = "Invoice" },
                new { SNo = 6, InvoiceNumber = "ETS-26006", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Completed", InvoiceFlowType = "Completed" },
                new { SNo = 7, InvoiceNumber = "ETS-26007", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Exchanged", InvoiceFlowType = "Return & Exchange" },
                new { SNo = 8, InvoiceNumber = "ETS-26008", CustomerName = "Harshit Pandey", PhoneNumber = "452-613-563", ItemCount = 5, TotalAmount = 500m, CreatedBy = "Jaya", CreatedDate = "9/2/2026", PaymentType = "Cash", Status = "Draft", InvoiceFlowType = "Invoice" }
            });
    }

    private static void BackfillBillingFlowData(IDbConnection connection)
    {
        connection.Execute(
            """
            UPDATE BillingInvoices
            SET InvoiceFlowType = 'Invoice'
            WHERE InvoiceFlowType IS NULL OR InvoiceFlowType = '';
            """);

        var specialRows = connection.ExecuteScalar<long>(
            """
            SELECT COUNT(1)
            FROM BillingInvoices
            WHERE InvoiceFlowType IN ('Return & Exchange', 'Completed');
            """);
        if (specialRows > 0)
        {
            return;
        }

        var completedId = connection.ExecuteScalar<long?>("SELECT Id FROM BillingInvoices ORDER BY Id DESC LIMIT 1;");
        var returnId = connection.ExecuteScalar<long?>("SELECT Id FROM BillingInvoices ORDER BY Id DESC LIMIT 1 OFFSET 1;");

        if (completedId is not null)
        {
            connection.Execute(
                """
                UPDATE BillingInvoices
                SET InvoiceFlowType = 'Completed',
                    Status = 'Completed'
                WHERE Id = @Id;
                """,
                new { Id = completedId.Value });
        }

        if (returnId is not null)
        {
            connection.Execute(
                """
                UPDATE BillingInvoices
                SET InvoiceFlowType = 'Return & Exchange',
                    Status = 'Exchanged'
                WHERE Id = @Id;
                """,
                new { Id = returnId.Value });
        }
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
            (SNo, Sku, ProductName, Category, Hsn, PurchasePrice, SellingPrice, Stock, Status, ReorderLevel)
            VALUES
            (@SNo, @Sku, @ProductName, @Category, @Hsn, @PurchasePrice, @SellingPrice, @Stock, @Status, @ReorderLevel);
            """,
            new[]
            {
                new { SNo = 1, Sku = "ITM-001", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good", ReorderLevel = 20 },
                new { SNo = 2, Sku = "ITM-002", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 40, Status = "Low", ReorderLevel = 50 },
                new { SNo = 3, Sku = "ITM-003", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 5, Status = "Critical", ReorderLevel = 50 },
                new { SNo = 4, Sku = "ITM-004", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good", ReorderLevel = 20 },
                new { SNo = 5, Sku = "ITM-005", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good", ReorderLevel = 20 },
                new { SNo = 6, Sku = "ITM-006", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good", ReorderLevel = 20 },
                new { SNo = 7, Sku = "ITM-007", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good", ReorderLevel = 20 },
                new { SNo = 8, Sku = "ITM-008", ProductName = "LED-Bulb 9w", Category = "LED & Lighting", Hsn = "85395000", PurchasePrice = 120m, SellingPrice = 140m, Stock = 450, Status = "Good", ReorderLevel = 20 }
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

    private static void SeedVendorsAndPurchases(IDbConnection connection)
    {
        if (!TableHasRows(connection, "Vendors"))
        {
            connection.Execute(
                """
                INSERT INTO Vendors
                (SNo, VendorName, CompanyName, PhoneNumber, EmailAddress, OutstandingBalance, Status, Address, AccountName, AccountNumber)
                VALUES
                (@SNo, @VendorName, @CompanyName, @PhoneNumber, @EmailAddress, @OutstandingBalance, @Status, @Address, @AccountName, @AccountNumber);
                """,
                new[]
                {
                    new { SNo = 1, VendorName = "Harshit Pandey", CompanyName = "Harshit01Imports", PhoneNumber = "555-533-6515", EmailAddress = "harshit@mail.com", OutstandingBalance = 3220m, Status = "Draft", Address = "123 Business Avenue", AccountName = "Harshit Pandey", AccountNumber = "0000 0000 0000" },
                    new { SNo = 2, VendorName = "Industrial Solutions Ltd.", CompanyName = "Industrial Solutions Ltd.", PhoneNumber = "555-113-7788", EmailAddress = "contact@isl.com", OutstandingBalance = 900m, Status = "Active", Address = "Warehouse District", AccountName = "Industrial Solutions Ltd.", AccountNumber = "1000 2300 0044" }
                });
        }

        if (!TableHasRows(connection, "PurchaseInvoices"))
        {
            connection.Execute(
                """
                INSERT INTO PurchaseInvoices
                (SNo, PONumber, VendorName, TotalProduct, TotalQuantity, TotalAmount, OrderDate, Status, ProductName, Quantity, Price)
                VALUES
                (@SNo, @PONumber, @VendorName, @TotalProduct, @TotalQuantity, @TotalAmount, @OrderDate, @Status, @ProductName, @Quantity, @Price);
                """,
                new[]
                {
                    new { SNo = 1, PONumber = "PO-2025-000024", VendorName = "Harshit Pandey", TotalProduct = 5, TotalQuantity = 155, TotalAmount = 3220m, OrderDate = "09/05/2026", Status = "Pending", ProductName = "9W Light Bulb", Quantity = 155, Price = 20.77m },
                    new { SNo = 2, PONumber = "PO-2025-000025", VendorName = "Industrial Solutions Ltd.", TotalProduct = 3, TotalQuantity = 60, TotalAmount = 1250m, OrderDate = "10/05/2026", Status = "Pending", ProductName = "Safety Goggles", Quantity = 60, Price = 20.83m }
                });
        }

        if (!TableHasRows(connection, "PurchaseReturns"))
        {
            connection.Execute(
                """
                INSERT INTO PurchaseReturns
                (SNo, PONumber, VendorName, PurchasedQuantity, ReturnQuantity, ReturnTotal, OrderDate, Status, InvoiceNumber)
                VALUES
                (@SNo, @PONumber, @VendorName, @PurchasedQuantity, @ReturnQuantity, @ReturnTotal, @OrderDate, @Status, @InvoiceNumber);
                """,
                new[]
                {
                    new { SNo = 1, PONumber = "PO-2025-000024", VendorName = "Harshit Pandey", PurchasedQuantity = 155, ReturnQuantity = 155, ReturnTotal = 3220m, OrderDate = "09/05/2026", Status = "Pending", InvoiceNumber = "INV-2023-0892" }
                });
        }

        if (!TableHasRows(connection, "VendorPayments"))
        {
            connection.Execute(
                """
                INSERT INTO VendorPayments
                (SNo, PONumber, VendorName, PaymentStatus, PaidAmount, PendingAmount, TotalAmount, DueDate, LastPaymentDate, PaymentMode)
                VALUES
                (@SNo, @PONumber, @VendorName, @PaymentStatus, @PaidAmount, @PendingAmount, @TotalAmount, @DueDate, @LastPaymentDate, @PaymentMode);
                """,
                new[]
                {
                    new { SNo = 1, PONumber = "PO-2025-000024", VendorName = "Harshit Pandey", PaymentStatus = "Paid", PaidAmount = 3000m, PendingAmount = 200m, TotalAmount = 3220m, DueDate = "12/05/2026", LastPaymentDate = "12/05/2026", PaymentMode = "Bank Transfer" }
                });
        }
    }

    private static void SeedExpenses(IDbConnection connection)
    {
        if (!TableHasRows(connection, "ExpenseSummaryCards"))
        {
            connection.Execute(
                """
                INSERT INTO ExpenseSummaryCards (Title, Amount, AccentColor, IconGlyph)
                VALUES (@Title, @Amount, @AccentColor, @IconGlyph);
                """,
                new[]
                {
                    new { Title = "Total Expenses (This Month)", Amount = 99000m, AccentColor = "#D8E9FB", IconGlyph = "\uEAFD" },
                    new { Title = "Rent", Amount = 25000m, AccentColor = "#F2DEE0", IconGlyph = "\uE80F" },
                    new { Title = "Electricity", Amount = 4500m, AccentColor = "#F3EFCF", IconGlyph = "\uE945" },
                    new { Title = "Salary", Amount = 45000m, AccentColor = "#D9F2DE", IconGlyph = "\uE77B" }
                });
        }

        if (!TableHasRows(connection, "Expenses"))
        {
            connection.Execute(
                """
                INSERT INTO Expenses (ExpenseId, ExpenseDate, Category, Description, Amount, PaidFrom)
                VALUES (@ExpenseId, @ExpenseDate, @Category, @Description, @Amount, @PaidFrom);
                """,
                new[]
                {
                    new { ExpenseId = "ETS-26001", ExpenseDate = "22/10/2026", Category = "Electricity", Description = "Monthly electricity bill", Amount = 4500m, PaidFrom = "Cash Account" }
                });
        }

        if (!TableHasRows(connection, "ExpenseCategoryBreakdown"))
        {
            connection.Execute(
                """
                INSERT INTO ExpenseCategoryBreakdown (Category, Percentage)
                VALUES (@Category, @Percentage);
                """,
                new[]
                {
                    new { Category = "Payroll & Salary", Percentage = 60 },
                    new { Category = "Rent & Office", Percentage = 25 },
                    new { Category = "Marketing & Sales", Percentage = 10 },
                    new { Category = "Utilities", Percentage = 5 },
                    new { Category = "Other", Percentage = 0 }
                });
        }
    }

    private static void SeedReports(IDbConnection connection)
    {
        if (!TableHasRows(connection, "ReportMetricCards"))
        {
            connection.Execute(
                """
                INSERT INTO ReportMetricCards (ReportType, Title, DisplayValue, TrendText, AccentColor)
                VALUES (@ReportType, @Title, @DisplayValue, @TrendText, @AccentColor);
                """,
                new[]
                {
                    new { ReportType = "SalesReport", Title = "Total Sales", DisplayValue = "$ 2,27,980", TrendText = "+12.5% From Last Period", AccentColor = "#D8E9FB" },
                    new { ReportType = "SalesReport", Title = "Total Invoices", DisplayValue = "62", TrendText = "+8.2% From Last Period", AccentColor = "#F2DEE0" },
                    new { ReportType = "SalesReport", Title = "Items Sold", DisplayValue = "740", TrendText = "+15.3% From Last Period", AccentColor = "#F3EFCF" },
                    new { ReportType = "SalesReport", Title = "Total Profit", DisplayValue = "$ 42,350", TrendText = "+10.8% From Last Period", AccentColor = "#D9F2DE" },

                    new { ReportType = "PurchaseReport", Title = "Total Purchases", DisplayValue = "$ 2,27,980", TrendText = "+12.5% From Last Period", AccentColor = "#D8E9FB" },
                    new { ReportType = "PurchaseReport", Title = "Total Purchase Invoices", DisplayValue = "48", TrendText = "+8.2% From Last Period", AccentColor = "#F2DEE0" },
                    new { ReportType = "PurchaseReport", Title = "Items Purchased", DisplayValue = "740", TrendText = "+15.3% From Last Period", AccentColor = "#F3EFCF" },
                    new { ReportType = "PurchaseReport", Title = "Top Supplier", DisplayValue = "Harshit", TrendText = "+10.8% From Last Period", AccentColor = "#D9F2DE" },

                    new { ReportType = "StockReport", Title = "Total Products", DisplayValue = "186", TrendText = "+4 New This Period", AccentColor = "#D8E9FB" },
                    new { ReportType = "StockReport", Title = "Total Stock Quantity", DisplayValue = "3,420", TrendText = "+8.5% From Last Period", AccentColor = "#F2DEE0" },
                    new { ReportType = "StockReport", Title = "Stock Value (Cost)", DisplayValue = "$ 1 240 500", TrendText = "+6.3% From Last Period", AccentColor = "#F3EFCF" },
                    new { ReportType = "StockReport", Title = "Stock Value (Selling)", DisplayValue = "$ 1 865 200", TrendText = "+7.9% From Last Period", AccentColor = "#D9F2DE" },

                    new { ReportType = "CustomerOutstanding", Title = "Total Credit Sales", DisplayValue = "$ 517 500", TrendText = "+8.4% From Last Period", AccentColor = "#D8E9FB" },
                    new { ReportType = "CustomerOutstanding", Title = "Total Received", DisplayValue = "$ 235 000", TrendText = "+5.2% From Last Period", AccentColor = "#F2DEE0" },
                    new { ReportType = "CustomerOutstanding", Title = "Total Outstanding", DisplayValue = "$ 282 500", TrendText = "-2.1% From Last Period", AccentColor = "#F3EFCF" },
                    new { ReportType = "CustomerOutstanding", Title = "Overdue Amount", DisplayValue = "$ 124 000", TrendText = "-6.8% From Last Period", AccentColor = "#D9F2DE" },

                    new { ReportType = "VendorOutstanding", Title = "Total Purchases", DisplayValue = "$ 1 240 000", TrendText = "+6.3% From Last Period", AccentColor = "#D8E9FB" },
                    new { ReportType = "VendorOutstanding", Title = "Total Paid To Vendors", DisplayValue = "$ 860 000", TrendText = "+4.8% From Last Period", AccentColor = "#F2DEE0" },
                    new { ReportType = "VendorOutstanding", Title = "Total Outstanding", DisplayValue = "$ 282 500", TrendText = "-3.5% From Last Period", AccentColor = "#F3EFCF" },
                    new { ReportType = "VendorOutstanding", Title = "Overdue Payables", DisplayValue = "$ 150 000", TrendText = "-5.2% From Last Period", AccentColor = "#D9F2DE" },

                    new { ReportType = "ProfitAndLoss", Title = "Total Revenue", DisplayValue = "$ 1 240 000", TrendText = "+6.3% From Last Period", AccentColor = "#D8E9FB" },
                    new { ReportType = "ProfitAndLoss", Title = "Total Cost", DisplayValue = "$ 860 000", TrendText = "+4.8% From Last Period", AccentColor = "#F2DEE0" },
                    new { ReportType = "ProfitAndLoss", Title = "Gross Profit", DisplayValue = "$ 282 500", TrendText = "-3.5% From Last Period", AccentColor = "#F3EFCF" },
                    new { ReportType = "ProfitAndLoss", Title = "Profit Margin", DisplayValue = "44.71%", TrendText = "-5.2% From Last Period", AccentColor = "#D9F2DE" }
                });
        }

        if (!TableHasRows(connection, "SalesReportRows"))
        {
            connection.Execute(
                """
                INSERT INTO SalesReportRows
                (Date, BillNo, CustomerName, ItemName, Quantity, Rate, Amount, PaymentStatus, PaymentMode, Profit)
                VALUES
                (@Date, @BillNo, @CustomerName, @ItemName, @Quantity, @Rate, @Amount, @PaymentStatus, @PaymentMode, @Profit);
                """,
                new[]
                {
                    new { Date = "22/10/2026", BillNo = "ETS-00123", CustomerName = "Harshit Pandey", ItemName = "9w Light Bulb", Quantity = 10, Rate = 300m, Amount = 3000m, PaymentStatus = "Paid", PaymentMode = "Cash", Profit = 3000m }
                });
        }

        if (!TableHasRows(connection, "PurchaseReportRows"))
        {
            connection.Execute(
                """
                INSERT INTO PurchaseReportRows
                (Date, BillNo, VendorName, ItemName, Quantity, PurchasePrice, TotalAmount, PaymentStatus)
                VALUES
                (@Date, @BillNo, @VendorName, @ItemName, @Quantity, @PurchasePrice, @TotalAmount, @PaymentStatus);
                """,
                new[]
                {
                    new { Date = "22/10/2026", BillNo = "ETS-00123", VendorName = "Harshit Pandey", ItemName = "9w Light Bulb", Quantity = 10, PurchasePrice = 300m, TotalAmount = 3000m, PaymentStatus = "Paid" }
                });
        }

        if (!TableHasRows(connection, "StockReportRows"))
        {
            connection.Execute(
                """
                INSERT INTO StockReportRows
                (ItemName, Category, OpeningStock, PurchaseQuantity, SalesQuantity, CurrentStock, PurchaseValue, SalesValue, ProfitMargin)
                VALUES
                (@ItemName, @Category, @OpeningStock, @PurchaseQuantity, @SalesQuantity, @CurrentStock, @PurchaseValue, @SalesValue, @ProfitMargin);
                """,
                new[]
                {
                    new { ItemName = "9w Light Bulb", Category = "Electric", OpeningStock = 100, PurchaseQuantity = 50, SalesQuantity = 47, CurrentStock = 103, PurchaseValue = 1.5m, SalesValue = 2.5m, ProfitMargin = 1m }
                });
        }

        if (!TableHasRows(connection, "CustomerOutstandingRows"))
        {
            connection.Execute(
                """
                INSERT INTO CustomerOutstandingRows
                (CustomerName, InvoiceNo, InvoiceDate, TotalAmount, PaidAmount, Balance)
                VALUES
                (@CustomerName, @InvoiceNo, @InvoiceDate, @TotalAmount, @PaidAmount, @Balance);
                """,
                new[]
                {
                    new { CustomerName = "Harshit Pandey", InvoiceNo = "ETS-26001", InvoiceDate = "17/02/2026", TotalAmount = 350m, PaidAmount = 50m, Balance = 300m }
                });
        }

        if (!TableHasRows(connection, "VendorOutstandingRows"))
        {
            connection.Execute(
                """
                INSERT INTO VendorOutstandingRows
                (VendorName, BillNo, BillDate, TotalAmount, PaidAmount, Balance)
                VALUES
                (@VendorName, @BillNo, @BillDate, @TotalAmount, @PaidAmount, @Balance);
                """,
                new[]
                {
                    new { VendorName = "Harshit Pandey", BillNo = "ETS-26001", BillDate = "17/02/2026", TotalAmount = 350m, PaidAmount = 50m, Balance = 300m }
                });
        }

        if (!TableHasRows(connection, "ProfitLossRows"))
        {
            connection.Execute(
                """
                INSERT INTO ProfitLossRows
                (Number, Product, UnitsSold, CostPrice, SalesPrice, TotalProfit, MarginPercent)
                VALUES
                (@Number, @Product, @UnitsSold, @CostPrice, @SalesPrice, @TotalProfit, @MarginPercent);
                """,
                new[]
                {
                    new { Number = 1, Product = "Harshit Pandey", UnitsSold = 40, CostPrice = 5m, SalesPrice = 10m, TotalProfit = 400m, MarginPercent = 50m }
                });
        }

        if (!TableHasRows(connection, "BalanceSheetEntries"))
        {
            connection.Execute(
                """
                INSERT INTO BalanceSheetEntries (EntryType, Section, AccountName, Amount)
                VALUES (@EntryType, @Section, @AccountName, @Amount);
                """,
                new[]
                {
                    new { EntryType = "Asset", Section = "Current Assets", AccountName = "Cash in Hand", Amount = 25000m },
                    new { EntryType = "Asset", Section = "Current Assets", AccountName = "HDFC Bank", Amount = 85000m },
                    new { EntryType = "Asset", Section = "Current Assets", AccountName = "Closing Stock", Amount = 45000m },
                    new { EntryType = "Asset", Section = "Current Assets", AccountName = "Customer Outstanding", Amount = 28000m },

                    new { EntryType = "Liability", Section = "Current Liabilities", AccountName = "Vendor Outstanding", Amount = 35000m },
                    new { EntryType = "Liability", Section = "Current Liabilities", AccountName = "Total Liabilities", Amount = 35000m },
                    new { EntryType = "Liability", Section = "Equity", AccountName = "Owner Capital Account", Amount = 100000m },
                    new { EntryType = "Liability", Section = "Equity", AccountName = "Retained Earnings (Profit)", Amount = 48000m }
                });
        }

        if (!TableHasRows(connection, "BalanceSheetTotals"))
        {
            connection.Execute(
                """
                INSERT INTO BalanceSheetTotals (TotalAssets, TotalLiabilitiesAndEquity)
                VALUES (@TotalAssets, @TotalLiabilitiesAndEquity);
                """,
                new[]
                {
                    new { TotalAssets = 183000m, TotalLiabilitiesAndEquity = 183000m }
                });
        }
    }

    private static void SeedSettings(IDbConnection connection)
    {
        if (!TableHasRows(connection, "CompanyProfileSettings"))
        {
            connection.Execute(
                """
                INSERT INTO CompanyProfileSettings
                (CompanyName, MobileNumber, EmailAddress, Address, LogoPath)
                VALUES
                (@CompanyName, @MobileNumber, @EmailAddress, @Address, @LogoPath);
                """,
                new[]
                {
                    new
                    {
                        CompanyName = "Acme Global Solutions Inc.",
                        MobileNumber = "+1 (212) 555-0198",
                        EmailAddress = "finance@acme-global.com",
                        Address = "1234 Enterprise Way, Suite 500\nNew York, NY 10001\nUnited States",
                        LogoPath = string.Empty
                    }
                });
        }

        if (!TableHasRows(connection, "LedgerAccounts"))
        {
            connection.Execute(
                """
                INSERT INTO LedgerAccounts (AccountName, AccountType, OpeningBalance, CreatedDate)
                VALUES (@AccountName, @AccountType, @OpeningBalance, @CreatedDate);
                """,
                new[]
                {
                    new { AccountName = "Cash Account", AccountType = "Cash", OpeningBalance = 45000m, CreatedDate = "Jan 12, 2024" },
                    new { AccountName = "Bank Account", AccountType = "Bank", OpeningBalance = 820000m, CreatedDate = "Jan 12, 2024" },
                    new { AccountName = "Owner Capital", AccountType = "Capital", OpeningBalance = 1000000m, CreatedDate = "Jan 15, 2024" },
                    new { AccountName = "Sales Account", AccountType = "Sales", OpeningBalance = 0m, CreatedDate = "Feb 01, 2024" },
                    new { AccountName = "Purchase Account", AccountType = "Purchase", OpeningBalance = 0m, CreatedDate = "Feb 01, 2024" },
                    new { AccountName = "Expense Account", AccountType = "Expense", OpeningBalance = 15400m, CreatedDate = "Feb 05, 2024" }
                });
        }

        if (!TableHasRows(connection, "AppUsers"))
        {
            connection.Execute(
                """
                INSERT INTO AppUsers (UserName, EmailAddress, MobileNumber, Role, IsActive, PasswordHash)
                VALUES (@UserName, @EmailAddress, @MobileNumber, @Role, @IsActive, @PasswordHash);
                """,
                new[]
                {
                    new { UserName = "Alex Johnson", EmailAddress = "alex.j@enterprise.com", MobileNumber = "+1 (555) 123-4567", Role = "Owner", IsActive = 1, PasswordHash = "hash-owner" },
                    new { UserName = "Sarah Miller", EmailAddress = "s.miller@enterprise.com", MobileNumber = "+1 (555) 987-6543", Role = "Accountant", IsActive = 1, PasswordHash = "hash-accountant" },
                    new { UserName = "Marcus Chen", EmailAddress = "m.chen@enterprise.com", MobileNumber = "+1 (555) 444-2211", Role = "Billing Operator", IsActive = 0, PasswordHash = "hash-billing" },
                    new { UserName = "David Wright", EmailAddress = "david.w@enterprise.com", MobileNumber = "+1 (555) 777-3322", Role = "Accountant", IsActive = 1, PasswordHash = "hash-accountant-2" }
                });
        }

        if (!TableHasRows(connection, "BackupStatus"))
        {
            connection.Execute(
                """
                INSERT INTO BackupStatus (CurrentStatus, LastSuccessfulBackup, NextScheduledBackup, AutoBackupEnabled)
                VALUES (@CurrentStatus, @LastSuccessfulBackup, @NextScheduledBackup, @AutoBackupEnabled);
                """,
                new[]
                {
                    new { CurrentStatus = "System Healthy", LastSuccessfulBackup = "17 Feb 2026, 10:30 AM", NextScheduledBackup = "Today, 00:00 UTC", AutoBackupEnabled = 1 }
                });
        }

        if (!TableHasRows(connection, "BackupLogs"))
        {
            connection.Execute(
                """
                INSERT INTO BackupLogs (Activity, DateAndTime, Size, Status)
                VALUES (@Activity, @DateAndTime, @Size, @Status);
                """,
                new[]
                {
                    new { Activity = "Automated Daily Backup", DateAndTime = "17 Feb 2026, 00:00 AM", Size = "42.5 MB", Status = "Success" },
                    new { Activity = "Manual Backup (Admin)", DateAndTime = "16 Feb 2026, 04:15 PM", Size = "41.8 MB", Status = "Success" },
                    new { Activity = "Automated Daily Backup", DateAndTime = "16 Feb 2026, 00:00 AM", Size = "--", Status = "Failed" },
                    new { Activity = "Automated Daily Backup", DateAndTime = "15 Feb 2026, 00:00 AM", Size = "41.2 MB", Status = "Success" }
                });
        }
    }

    private static bool TableHasRows(IDbConnection connection, string tableName)
    {
        var count = connection.ExecuteScalar<long>($"SELECT COUNT(1) FROM {tableName};");
        return count > 0;
    }
}
