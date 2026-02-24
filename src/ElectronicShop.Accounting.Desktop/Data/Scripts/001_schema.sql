PRAGMA foreign_keys = ON;

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
