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

    public IReadOnlyList<BillingInvoiceRow> GetBillingInvoices(string? invoiceFlowType = null)
    {
        var sql =
            """
            SELECT SNo, InvoiceNumber, CustomerName, PhoneNumber, ItemCount, TotalAmount, CreatedBy, CreatedDate, PaymentType, Status, InvoiceFlowType
            FROM BillingInvoices
            ORDER BY Id DESC;
            """;

        using var connection = CreateConnection();
        if (string.IsNullOrWhiteSpace(invoiceFlowType))
        {
            return connection.Query<BillingInvoiceRow>(sql).ToList();
        }

        var filteredSql =
            """
            SELECT SNo, InvoiceNumber, CustomerName, PhoneNumber, ItemCount, TotalAmount, CreatedBy, CreatedDate, PaymentType, Status, InvoiceFlowType
            FROM BillingInvoices
            WHERE InvoiceFlowType = @InvoiceFlowType
            ORDER BY Id DESC;
            """;
        return connection.Query<BillingInvoiceRow>(filteredSql, new { InvoiceFlowType = invoiceFlowType }).ToList();
    }

    public void AddBillingInvoice(AddBillingInvoiceInput input)
    {
        using var connection = CreateConnection();
        var sNo = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(SNo), 0) + 1 FROM BillingInvoices;");
        var invoiceNumber = $"ETS-{26000 + sNo}";

        connection.Execute(
            """
            INSERT INTO BillingInvoices
            (SNo, InvoiceNumber, CustomerName, PhoneNumber, ItemCount, TotalAmount, CreatedBy, CreatedDate, PaymentType, Status, InvoiceFlowType)
            VALUES
            (@SNo, @InvoiceNumber, @CustomerName, @PhoneNumber, @ItemCount, @TotalAmount, @CreatedBy, @CreatedDate, @PaymentType, @Status, @InvoiceFlowType);
            """,
            new
            {
                SNo = sNo,
                InvoiceNumber = invoiceNumber,
                input.CustomerName,
                input.PhoneNumber,
                ItemCount = input.ItemCount,
                TotalAmount = input.TotalAmount,
                CreatedBy = "System",
                CreatedDate = DateTime.Now.ToString("M/d/yyyy"),
                input.PaymentType,
                input.Status,
                input.InvoiceFlowType
            });
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

    public void AddInventoryItem(AddInventoryItemInput input)
    {
        using var connection = CreateConnection();
        var sNo = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(SNo), 0) + 1 FROM InventoryItems;");
        var sku = string.IsNullOrWhiteSpace(input.ProductCode) ? $"ITM-{sNo:000}" : input.ProductCode.ToUpperInvariant();
        var status = input.OpeningStock <= 5 ? "Critical" : input.OpeningStock <= input.ReorderLevel ? "Low" : "Good";

        connection.Execute(
            """
            INSERT INTO InventoryItems
            (SNo, Sku, ProductName, Category, Hsn, PurchasePrice, SellingPrice, Stock, Status, ReorderLevel)
            VALUES
            (@SNo, @Sku, @ProductName, @Category, @Hsn, @PurchasePrice, @SellingPrice, @Stock, @Status, @ReorderLevel);
            """,
            new
            {
                SNo = sNo,
                Sku = sku,
                input.ProductName,
                input.Category,
                Hsn = "85395000",
                input.PurchasePrice,
                SellingPrice = input.SalesPrice,
                Stock = input.OpeningStock,
                Status = status,
                input.ReorderLevel
            });

        RefreshInventorySummary(connection);
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

    public void AddBankTransaction(AddBankTransactionInput input)
    {
        using var connection = CreateConnection();
        var nextId = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(Id), 0) + 1 FROM BankTransactions;");
        var transactionCode = $"ETS-{26000 + nextId}";
        var normalizedType = NormalizeTransactionType(input.TransactionType);

        connection.Execute(
            """
            INSERT INTO BankTransactions
            (TransactionCode, TransactionDate, Description, AccountName, TransactionType, Amount)
            VALUES
            (@TransactionCode, @TransactionDate, @Description, @AccountName, @TransactionType, @Amount);
            """,
            new
            {
                TransactionCode = transactionCode,
                input.TransactionDate,
                input.Description,
                input.AccountName,
                TransactionType = normalizedType,
                input.Amount
            });

        if (normalizedType.Equals("Credit", StringComparison.OrdinalIgnoreCase))
        {
            connection.Execute(
                """
                UPDATE BankAccounts
                SET Balance = Balance + @Amount
                WHERE AccountName = @AccountName;
                """,
                new { input.Amount, input.AccountName });
        }
        else
        {
            connection.Execute(
                """
                UPDATE BankAccounts
                SET Balance = Balance - @Amount
                WHERE AccountName = @AccountName;
                """,
                new { input.Amount, input.AccountName });
        }
    }

    public IReadOnlyList<VendorRow> GetVendors()
    {
        const string sql =
            """
            SELECT SNo, VendorName, CompanyName, PhoneNumber, EmailAddress, OutstandingBalance, Status
            FROM Vendors
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<VendorRow>(sql).ToList();
    }

    public IReadOnlyList<PurchaseInvoiceRow> GetPurchaseInvoices()
    {
        const string sql =
            """
            SELECT SNo, PONumber, VendorName, TotalProduct, TotalQuantity, TotalAmount, OrderDate, Status
            FROM PurchaseInvoices
            ORDER BY Id DESC;
            """;

        using var connection = CreateConnection();
        return connection.Query<PurchaseInvoiceRow>(sql).ToList();
    }

    public IReadOnlyList<PurchaseReturnRow> GetPurchaseReturns()
    {
        const string sql =
            """
            SELECT SNo, PONumber, VendorName, PurchasedQuantity, ReturnQuantity, ReturnTotal, OrderDate, Status
            FROM PurchaseReturns
            ORDER BY Id DESC;
            """;

        using var connection = CreateConnection();
        return connection.Query<PurchaseReturnRow>(sql).ToList();
    }

    public IReadOnlyList<VendorPayBillRow> GetVendorPayments()
    {
        const string sql =
            """
            SELECT SNo, PONumber, VendorName, PaymentStatus, PaidAmount, PendingAmount, TotalAmount, DueDate, LastPaymentDate
            FROM VendorPayments
            ORDER BY Id DESC;
            """;

        using var connection = CreateConnection();
        return connection.Query<VendorPayBillRow>(sql).ToList();
    }

    public void AddVendor(AddVendorInput input)
    {
        using var connection = CreateConnection();
        var sNo = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(SNo), 0) + 1 FROM Vendors;");

        connection.Execute(
            """
            INSERT INTO Vendors
            (SNo, VendorName, CompanyName, PhoneNumber, EmailAddress, OutstandingBalance, Status, Address, AccountName, AccountNumber)
            VALUES
            (@SNo, @VendorName, @CompanyName, @PhoneNumber, @EmailAddress, @OutstandingBalance, @Status, @Address, @AccountName, @AccountNumber);
            """,
            new
            {
                SNo = sNo,
                input.VendorName,
                input.CompanyName,
                input.PhoneNumber,
                input.EmailAddress,
                OutstandingBalance = 0m,
                Status = "Draft",
                input.Address,
                input.AccountName,
                input.AccountNumber
            });
    }

    public void AddPurchaseInvoice(AddPurchaseInvoiceInput input)
    {
        using var connection = CreateConnection();
        var sNo = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(SNo), 0) + 1 FROM PurchaseInvoices;");
        var poNumber = $"PO-2025-{sNo:000000}";
        var totalAmount = input.Quantity * input.Price;

        connection.Execute(
            """
            INSERT INTO PurchaseInvoices
            (SNo, PONumber, VendorName, TotalProduct, TotalQuantity, TotalAmount, OrderDate, Status, ProductName, Quantity, Price)
            VALUES
            (@SNo, @PONumber, @VendorName, @TotalProduct, @TotalQuantity, @TotalAmount, @OrderDate, @Status, @ProductName, @Quantity, @Price);
            """,
            new
            {
                SNo = sNo,
                PONumber = poNumber,
                input.VendorName,
                TotalProduct = 1,
                TotalQuantity = input.Quantity,
                TotalAmount = totalAmount,
                OrderDate = input.PurchaseDate,
                Status = input.PurchaseStatus,
                input.ProductName,
                Quantity = input.Quantity,
                input.Price
            });

        connection.Execute(
            """
            INSERT INTO VendorPayments
            (SNo, PONumber, VendorName, PaymentStatus, PaidAmount, PendingAmount, TotalAmount, DueDate, LastPaymentDate, PaymentMode)
            VALUES
            (@SNo, @PONumber, @VendorName, @PaymentStatus, @PaidAmount, @PendingAmount, @TotalAmount, @DueDate, @LastPaymentDate, @PaymentMode);
            """,
            new
            {
                SNo = sNo,
                PONumber = poNumber,
                input.VendorName,
                PaymentStatus = "Pending",
                PaidAmount = 0m,
                PendingAmount = totalAmount,
                TotalAmount = totalAmount,
                DueDate = input.PurchaseDate,
                LastPaymentDate = input.PurchaseDate,
                PaymentMode = "Bank Transfer"
            });
    }

    public void AddPurchaseReturn(AddPurchaseReturnInput input)
    {
        using var connection = CreateConnection();
        var sNo = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(SNo), 0) + 1 FROM PurchaseReturns;");
        var returnTotal = input.ReturnQuantity * input.UnitPrice;

        connection.Execute(
            """
            INSERT INTO PurchaseReturns
            (SNo, PONumber, VendorName, PurchasedQuantity, ReturnQuantity, ReturnTotal, OrderDate, Status, InvoiceNumber)
            VALUES
            (@SNo, @PONumber, @VendorName, @PurchasedQuantity, @ReturnQuantity, @ReturnTotal, @OrderDate, @Status, @InvoiceNumber);
            """,
            new
            {
                SNo = sNo,
                PONumber = string.IsNullOrWhiteSpace(input.InvoiceNumber) ? $"PO-2025-{sNo:000000}" : input.InvoiceNumber,
                input.VendorName,
                PurchasedQuantity = input.ReturnQuantity,
                ReturnQuantity = input.ReturnQuantity,
                ReturnTotal = returnTotal,
                OrderDate = input.ReturnDate,
                Status = "Pending",
                InvoiceNumber = input.InvoiceNumber
            });
    }

    public void AddVendorPayment(AddVendorPaymentInput input)
    {
        using var connection = CreateConnection();
        var row = connection.QueryFirstOrDefault<VendorPaymentState>(
            """
            SELECT Id, SNo, PONumber, VendorName, PaidAmount, PendingAmount, TotalAmount
            FROM VendorPayments
            WHERE PONumber = @PONumber
            ORDER BY Id DESC
            LIMIT 1;
            """,
            new { PONumber = input.InvoiceNumber });

        if (row is null)
        {
            var sNo = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(SNo), 0) + 1 FROM VendorPayments;");
            connection.Execute(
                """
                INSERT INTO VendorPayments
                (SNo, PONumber, VendorName, PaymentStatus, PaidAmount, PendingAmount, TotalAmount, DueDate, LastPaymentDate, PaymentMode)
                VALUES
                (@SNo, @PONumber, @VendorName, @PaymentStatus, @PaidAmount, @PendingAmount, @TotalAmount, @DueDate, @LastPaymentDate, @PaymentMode);
                """,
                new
                {
                    SNo = sNo,
                    PONumber = input.InvoiceNumber,
                    input.VendorName,
                    PaymentStatus = "Paid",
                    PaidAmount = input.Amount,
                    PendingAmount = 0m,
                    TotalAmount = input.Amount,
                    DueDate = input.PaymentDate,
                    LastPaymentDate = input.PaymentDate,
                    input.PaymentMode
                });
            return;
        }

        var newPaid = row.PaidAmount + input.Amount;
        var newPending = Math.Max(0, row.TotalAmount - newPaid);
        var paymentStatus = newPending == 0 ? "Paid" : "Partial";

        connection.Execute(
            """
            UPDATE VendorPayments
            SET PaidAmount = @PaidAmount,
                PendingAmount = @PendingAmount,
                PaymentStatus = @PaymentStatus,
                LastPaymentDate = @LastPaymentDate,
                PaymentMode = @PaymentMode
            WHERE Id = @Id;
            """,
            new
            {
                Id = row.Id,
                PaidAmount = newPaid,
                PendingAmount = newPending,
                PaymentStatus = paymentStatus,
                LastPaymentDate = input.PaymentDate,
                input.PaymentMode
            });
    }

    public IReadOnlyList<ExpenseSummaryCard> GetExpenseSummaryCards()
    {
        const string sql =
            """
            SELECT Title, Amount, AccentColor, IconGlyph
            FROM ExpenseSummaryCards
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<ExpenseSummaryCard>(sql).ToList();
    }

    public IReadOnlyList<ExpenseRow> GetExpenses()
    {
        const string sql =
            """
            SELECT ExpenseId, ExpenseDate, Category, Description, Amount
            FROM Expenses
            ORDER BY Id DESC;
            """;

        using var connection = CreateConnection();
        return connection.Query<ExpenseRow>(sql).ToList();
    }

    public IReadOnlyList<ExpenseCategoryBreakdown> GetExpenseCategoryBreakdown()
    {
        const string sql =
            """
            SELECT Category, Percentage, 0 AS BarWidth
            FROM ExpenseCategoryBreakdown
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<ExpenseCategoryBreakdown>(sql).ToList();
    }

    public void AddExpense(AddExpenseInput input)
    {
        using var connection = CreateConnection();
        var nextId = connection.ExecuteScalar<int>("SELECT COALESCE(MAX(Id), 0) + 1 FROM Expenses;");
        var expenseId = $"ETS-{26000 + nextId}";

        connection.Execute(
            """
            INSERT INTO Expenses
            (ExpenseId, ExpenseDate, Category, Description, Amount, PaidFrom)
            VALUES
            (@ExpenseId, @ExpenseDate, @Category, @Description, @Amount, @PaidFrom);
            """,
            new
            {
                ExpenseId = expenseId,
                input.ExpenseDate,
                input.Category,
                Description = input.Title,
                input.Amount,
                input.PaidFrom
            });

        RefreshExpenseSummary(connection);
    }

    public IReadOnlyList<ReportMetricCard> GetReportMetricCards(ReportTab tab)
    {
        const string sql =
            """
            SELECT Title, DisplayValue, TrendText, AccentColor
            FROM ReportMetricCards
            WHERE ReportType = @ReportType
            ORDER BY Id;
            """;

        using var connection = CreateConnection();
        return connection.Query<ReportMetricCard>(sql, new { ReportType = tab.ToString() }).ToList();
    }

    public IReadOnlyList<SalesReportRow> GetSalesReportRows()
    {
        const string sql = "SELECT Date, BillNo, CustomerName, ItemName, Quantity, Rate, Amount, PaymentStatus, PaymentMode, Profit FROM SalesReportRows ORDER BY Id DESC;";
        using var connection = CreateConnection();
        return connection.Query<SalesReportRow>(sql).ToList();
    }

    public IReadOnlyList<PurchaseReportRow> GetPurchaseReportRows()
    {
        const string sql = "SELECT Date, BillNo, VendorName, ItemName, Quantity, PurchasePrice, TotalAmount, PaymentStatus FROM PurchaseReportRows ORDER BY Id DESC;";
        using var connection = CreateConnection();
        return connection.Query<PurchaseReportRow>(sql).ToList();
    }

    public IReadOnlyList<StockReportRow> GetStockReportRows()
    {
        const string sql = "SELECT ItemName, Category, OpeningStock, PurchaseQuantity, SalesQuantity, CurrentStock, PurchaseValue, SalesValue, ProfitMargin FROM StockReportRows ORDER BY Id DESC;";
        using var connection = CreateConnection();
        return connection.Query<StockReportRow>(sql).ToList();
    }

    public IReadOnlyList<CustomerOutstandingRow> GetCustomerOutstandingRows()
    {
        const string sql = "SELECT CustomerName, InvoiceNo, InvoiceDate, TotalAmount, PaidAmount, Balance FROM CustomerOutstandingRows ORDER BY Id DESC;";
        using var connection = CreateConnection();
        return connection.Query<CustomerOutstandingRow>(sql).ToList();
    }

    public IReadOnlyList<VendorOutstandingRow> GetVendorOutstandingRows()
    {
        const string sql = "SELECT VendorName, BillNo, BillDate, TotalAmount, PaidAmount, Balance FROM VendorOutstandingRows ORDER BY Id DESC;";
        using var connection = CreateConnection();
        return connection.Query<VendorOutstandingRow>(sql).ToList();
    }

    public IReadOnlyList<ProfitLossRow> GetProfitLossRows()
    {
        const string sql = "SELECT Number, Product, UnitsSold, CostPrice, SalesPrice, TotalProfit, MarginPercent FROM ProfitLossRows ORDER BY Id DESC;";
        using var connection = CreateConnection();
        return connection.Query<ProfitLossRow>(sql).ToList();
    }

    public IReadOnlyList<BalanceSheetEntry> GetBalanceSheetEntries(string entryType)
    {
        const string sql =
            """
            SELECT Section, AccountName, Amount
            FROM BalanceSheetEntries
            WHERE EntryType = @EntryType
            ORDER BY Id;
            """;
        using var connection = CreateConnection();
        return connection.Query<BalanceSheetEntry>(sql, new { EntryType = entryType }).ToList();
    }

    public BalanceSheetTotals GetBalanceSheetTotals()
    {
        const string sql = "SELECT TotalAssets, TotalLiabilitiesAndEquity FROM BalanceSheetTotals ORDER BY Id DESC LIMIT 1;";
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<BalanceSheetTotals>(sql) ?? new BalanceSheetTotals();
    }

    public CompanyProfileSettings GetCompanyProfile()
    {
        const string sql = "SELECT CompanyName, MobileNumber, EmailAddress, Address, LogoPath FROM CompanyProfileSettings ORDER BY Id DESC LIMIT 1;";
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<CompanyProfileSettings>(sql) ?? new CompanyProfileSettings();
    }

    public void SaveCompanyProfile(CompanyProfileSettings profile)
    {
        using var connection = CreateConnection();
        var existingId = connection.ExecuteScalar<long?>("SELECT Id FROM CompanyProfileSettings ORDER BY Id DESC LIMIT 1;");
        if (existingId is null)
        {
            connection.Execute(
                """
                INSERT INTO CompanyProfileSettings (CompanyName, MobileNumber, EmailAddress, Address, LogoPath)
                VALUES (@CompanyName, @MobileNumber, @EmailAddress, @Address, @LogoPath);
                """,
                profile);
            return;
        }

        connection.Execute(
            """
            UPDATE CompanyProfileSettings
            SET CompanyName = @CompanyName,
                MobileNumber = @MobileNumber,
                EmailAddress = @EmailAddress,
                Address = @Address,
                LogoPath = @LogoPath
            WHERE Id = @Id;
            """,
            new
            {
                Id = existingId.Value,
                profile.CompanyName,
                profile.MobileNumber,
                profile.EmailAddress,
                profile.Address,
                profile.LogoPath
            });
    }

    public IReadOnlyList<LedgerAccountRow> GetLedgerAccounts()
    {
        const string sql = "SELECT AccountName, AccountType, OpeningBalance, CreatedDate FROM LedgerAccounts ORDER BY Id;";
        using var connection = CreateConnection();
        return connection.Query<LedgerAccountRow>(sql).ToList();
    }

    public IReadOnlyList<AccountSummaryCard> GetAccountSummaryCards()
    {
        using var connection = CreateConnection();

        var assets = connection.ExecuteScalar<decimal>(
            """
            SELECT COALESCE(SUM(OpeningBalance), 0)
            FROM LedgerAccounts
            WHERE AccountType IN ('Cash', 'Bank');
            """);

        var liabilities = connection.ExecuteScalar<decimal>(
            """
            SELECT COALESCE(SUM(OpeningBalance), 0)
            FROM LedgerAccounts
            WHERE AccountType IN ('Liability', 'Payable');
            """);

        var capital = connection.ExecuteScalar<decimal>(
            """
            SELECT COALESCE(SUM(OpeningBalance), 0)
            FROM LedgerAccounts
            WHERE AccountType IN ('Capital', 'Sales');
            """);

        var expenses = connection.ExecuteScalar<decimal>(
            """
            SELECT COALESCE(SUM(OpeningBalance), 0)
            FROM LedgerAccounts
            WHERE AccountType IN ('Expense', 'Purchase');
            """);

        var netEquity = capital - expenses;
        return
        [
            new AccountSummaryCard { Title = "Total Assets", Amount = assets, AccentColor = "#D8E9FB" },
            new AccountSummaryCard { Title = "Total Liabilities", Amount = liabilities, AccentColor = "#F2DEE0" },
            new AccountSummaryCard { Title = "Net Equity", Amount = netEquity, AccentColor = "#DDEBFA" }
        ];
    }

    public void AddLedgerAccount(AddAccountInput input)
    {
        using var connection = CreateConnection();
        connection.Execute(
            """
            INSERT INTO LedgerAccounts (AccountName, AccountType, OpeningBalance, CreatedDate)
            VALUES (@AccountName, @AccountType, @OpeningBalance, @CreatedDate);
            """,
            new
            {
                input.AccountName,
                input.AccountType,
                input.OpeningBalance,
                CreatedDate = input.AsOfDate
            });
    }

    public IReadOnlyList<AppUserRow> GetAppUsers()
    {
        const string sql = "SELECT UserName, EmailAddress, MobileNumber, Role, IsActive FROM AppUsers ORDER BY Id;";
        using var connection = CreateConnection();
        return connection.Query<AppUserRow>(sql).ToList();
    }

    public void AddAppUser(AddUserInput input)
    {
        using var connection = CreateConnection();
        var emailPrefix = input.UserName.Replace(" ", ".", StringComparison.Ordinal).ToLowerInvariant();
        var email = $"{emailPrefix}@enterprise.com";

        connection.Execute(
            """
            INSERT INTO AppUsers (UserName, EmailAddress, MobileNumber, Role, IsActive, PasswordHash)
            VALUES (@UserName, @EmailAddress, @MobileNumber, @Role, @IsActive, @PasswordHash);
            """,
            new
            {
                input.UserName,
                EmailAddress = email,
                input.MobileNumber,
                input.Role,
                IsActive = 1,
                PasswordHash = input.Password
            });
    }

    public void UpdateAppUserStatus(string userName, bool isActive)
    {
        using var connection = CreateConnection();
        connection.Execute(
            """
            UPDATE AppUsers
            SET IsActive = @IsActive
            WHERE UserName = @UserName;
            """,
            new { UserName = userName, IsActive = isActive ? 1 : 0 });
    }

    public BackupStatus GetBackupStatus()
    {
        const string sql = "SELECT CurrentStatus, LastSuccessfulBackup, NextScheduledBackup, AutoBackupEnabled FROM BackupStatus ORDER BY Id DESC LIMIT 1;";
        using var connection = CreateConnection();
        return connection.QueryFirstOrDefault<BackupStatus>(sql) ?? new BackupStatus();
    }

    public IReadOnlyList<BackupLogRow> GetBackupLogs()
    {
        const string sql = "SELECT Activity, DateAndTime, Size, Status FROM BackupLogs ORDER BY Id DESC;";
        using var connection = CreateConnection();
        return connection.Query<BackupLogRow>(sql).ToList();
    }

    public void SetAutoBackupEnabled(bool isEnabled)
    {
        using var connection = CreateConnection();
        connection.Execute(
            """
            UPDATE BackupStatus
            SET AutoBackupEnabled = @AutoBackupEnabled;
            """,
            new { AutoBackupEnabled = isEnabled ? 1 : 0 });
    }

    public void AddBackupLog(string activity, string status)
    {
        using var connection = CreateConnection();
        connection.Execute(
            """
            INSERT INTO BackupLogs (Activity, DateAndTime, Size, Status)
            VALUES (@Activity, @DateAndTime, @Size, @Status);
            """,
            new
            {
                Activity = activity,
                DateAndTime = DateTime.Now.ToString("dd MMM yyyy, hh:mm tt"),
                Size = "41.0 MB",
                Status = status
            });
    }

    private static string NormalizeTransactionType(string type)
    {
        if (type.Equals("Received", StringComparison.OrdinalIgnoreCase) ||
            type.Equals("Credit", StringComparison.OrdinalIgnoreCase))
        {
            return "Credit";
        }

        return "Debit";
    }

    private static void RefreshInventorySummary(IDbConnection connection)
    {
        var totalItems = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM InventoryItems;");
        var lowStock = connection.ExecuteScalar<int>("SELECT COUNT(1) FROM InventoryItems WHERE Status IN ('Low', 'Critical');");
        var costValue = connection.ExecuteScalar<decimal>("SELECT COALESCE(SUM(PurchasePrice * Stock), 0) FROM InventoryItems;");
        var sellingValue = connection.ExecuteScalar<decimal>("SELECT COALESCE(SUM(SellingPrice * Stock), 0) FROM InventoryItems;");

        connection.Execute("DELETE FROM InventoryOverviewCards;");
        connection.Execute(
            """
            INSERT INTO InventoryOverviewCards (Title, DisplayValue, AccentColor, IconGlyph)
            VALUES (@Title, @DisplayValue, @AccentColor, @IconGlyph);
            """,
            new[]
            {
                new { Title = "Total Items", DisplayValue = totalItems.ToString(), AccentColor = "#D8E9FB", IconGlyph = "\uE7BF" },
                new { Title = "Low Stock Alert", DisplayValue = lowStock.ToString(), AccentColor = "#F2DEE0", IconGlyph = "\uE814" },
                new { Title = "Stock Value(Cost)", DisplayValue = costValue.ToString("C0"), AccentColor = "#F3EFCF", IconGlyph = "\uEAFD" },
                new { Title = "Stock Value (Selling)", DisplayValue = sellingValue.ToString("C0"), AccentColor = "#D9F2DE", IconGlyph = "\uE9D2" }
            });
    }

    private static void RefreshExpenseSummary(IDbConnection connection)
    {
        var total = connection.ExecuteScalar<decimal>("SELECT COALESCE(SUM(Amount), 0) FROM Expenses;");
        var rent = connection.ExecuteScalar<decimal>("SELECT COALESCE(SUM(Amount), 0) FROM Expenses WHERE Category LIKE '%Rent%';");
        var electricity = connection.ExecuteScalar<decimal>("SELECT COALESCE(SUM(Amount), 0) FROM Expenses WHERE Category LIKE '%Electric%';");
        var salary = connection.ExecuteScalar<decimal>("SELECT COALESCE(SUM(Amount), 0) FROM Expenses WHERE Category LIKE '%Salary%' OR Category LIKE '%Payroll%';");

        connection.Execute("DELETE FROM ExpenseSummaryCards;");
        connection.Execute(
            """
            INSERT INTO ExpenseSummaryCards (Title, Amount, AccentColor, IconGlyph)
            VALUES (@Title, @Amount, @AccentColor, @IconGlyph);
            """,
            new[]
            {
                new { Title = "Total Expenses (This Month)", Amount = total, AccentColor = "#D8E9FB", IconGlyph = "\uEAFD" },
                new { Title = "Rent", Amount = rent, AccentColor = "#F2DEE0", IconGlyph = "\uE80F" },
                new { Title = "Electricity", Amount = electricity, AccentColor = "#F3EFCF", IconGlyph = "\uE945" },
                new { Title = "Salary", Amount = salary, AccentColor = "#D9F2DE", IconGlyph = "\uE77B" }
            });
    }

    private SqliteConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }

    private sealed class VendorPaymentState
    {
        public long Id { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal PendingAmount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
