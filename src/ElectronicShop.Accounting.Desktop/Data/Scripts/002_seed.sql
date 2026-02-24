INSERT OR IGNORE INTO DashboardSummaryCards (Id, Title, Amount, ChangePercent, AccentColor, IconGlyph) VALUES
    (1, 'Total Sale', 74500, 21, '#D8E9FB', 'E8C7'),
    (2, 'Total Profit', 40352, 17, '#E5DCF9', 'E9D2'),
    (3, 'Pending Payment', 14515, 17, '#D9F2DE', 'E8AB'),
    (4, 'Cash Balance', 74500, 21, '#EFE9CD', 'EAFD'),
    (5, 'Bank Balance', 74500, 21, '#DDEBFA', 'E825'),
    (6, 'Low Stock Items', 14, 21, '#F4DDDF', 'EA39');

INSERT OR IGNORE INTO MonthlySalesTrend (Id, Month, Amount) VALUES
    (1, 'Jan', 48),
    (2, 'Feb', 72),
    (3, 'Mar', 14),
    (4, 'Apr', 32),
    (5, 'May', 90),
    (6, 'Jun', 24),
    (7, 'Jul', 49),
    (8, 'Aug', 70),
    (9, 'Sep', 49),
    (10, 'Oct', 49),
    (11, 'Nov', 66),
    (12, 'Dec', 50);

INSERT OR IGNORE INTO TopSellingItems (Id, Rank, ItemName, UnitsSold, Revenue) VALUES
    (1, 1, 'LED Bulb 9W', 245, 35500),
    (2, 2, 'LED Bulb 9W', 245, 35500),
    (3, 3, 'LED Bulb 9W', 245, 35500),
    (4, 4, 'LED Bulb 9W', 245, 35500),
    (5, 5, 'LED Bulb 9W', 245, 35500),
    (6, 6, 'LED Bulb 9W', 245, 35500),
    (7, 7, 'LED Bulb 9W', 245, 35500);

INSERT OR IGNORE INTO RecentInvoices (Id, InvoiceNumber, CustomerName, InvoiceDate, Amount, Status, PaymentType) VALUES
    (1, 'INV-2024-0156', 'Sharma Electricals', '2024-01-15', 12450, 'Paid', 'UPI'),
    (2, 'INV-2024-0157', 'Sharma Electricals', '2024-01-15', 12450, 'Paid', 'UPI'),
    (3, 'INV-2024-0158', 'Sharma Electricals', '2024-01-15', 12450, 'Paid', 'UPI'),
    (4, 'INV-2024-0159', 'Sharma Electricals', '2024-01-15', 12450, 'Paid', 'UPI'),
    (5, 'INV-2024-0160', 'Sharma Electricals', '2024-01-15', 12450, 'Paid', 'UPI'),
    (6, 'INV-2024-0161', 'Sharma Electricals', '2024-01-15', 12450, 'Paid', 'UPI');

INSERT OR IGNORE INTO BranchRevenue (Id, BranchName, Revenue, ChangePercent, AccentColor) VALUES
    (1, 'Branch 01', 35052, 21, '#DDEBFA'),
    (2, 'Branch 02', 35052, 21, '#EAE2FA'),
    (3, 'Branch 03', 35052, 21, '#F3EFCF');

INSERT OR IGNORE INTO BillingInvoices
    (Id, SNo, InvoiceNumber, CustomerName, PhoneNumber, ItemCount, TotalAmount, CreatedBy, CreatedDate, PaymentType, Status, InvoiceFlowType)
VALUES
    (1, 1, 'ETS-26001', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Draft', 'Invoice'),
    (2, 2, 'ETS-26002', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Draft', 'Invoice'),
    (3, 3, 'ETS-26003', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Draft', 'Invoice'),
    (4, 4, 'ETS-26004', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Draft', 'Invoice'),
    (5, 5, 'ETS-26005', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Draft', 'Invoice'),
    (6, 6, 'ETS-26006', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Completed', 'Completed'),
    (7, 7, 'ETS-26007', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Exchanged', 'Return & Exchange'),
    (8, 8, 'ETS-26008', 'Harshit Pandey', '452-613-563', 5, 500, 'Jaya', '9/2/2026', 'Cash', 'Draft', 'Invoice');

INSERT OR IGNORE INTO InventoryOverviewCards (Id, Title, DisplayValue, AccentColor, IconGlyph) VALUES
    (1, 'Total Items', '12', '#D8E9FB', 'E7BF'),
    (2, 'Low Stock Alert', '3', '#F2DEE0', 'E814'),
    (3, 'Stock Value(Cost)', '$2,451', '#F3EFCF', 'EAFD'),
    (4, 'Stock Value (Selling)', '$2,451', '#D9F2DE', 'E9D2');

INSERT OR IGNORE INTO InventoryItems
    (Id, SNo, Sku, ProductName, Category, Hsn, PurchasePrice, SellingPrice, Stock, Status, ReorderLevel)
VALUES
    (1, 1, 'ITM-001', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 450, 'Good', 20),
    (2, 2, 'ITM-002', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 40, 'Low', 50),
    (3, 3, 'ITM-003', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 5, 'Critical', 50),
    (4, 4, 'ITM-004', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 450, 'Good', 20),
    (5, 5, 'ITM-005', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 450, 'Good', 20),
    (6, 6, 'ITM-006', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 450, 'Good', 20),
    (7, 7, 'ITM-007', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 450, 'Good', 20),
    (8, 8, 'ITM-008', 'LED-Bulb 9w', 'LED & Lighting', '85395000', 120, 140, 450, 'Good', 20);

INSERT OR IGNORE INTO BankAccounts (Id, AccountName, Balance, IconGlyph) VALUES
    (1, 'Cash Account', 99000, 'E8C7'),
    (2, 'HDFC Bank - Current', 99000, 'E825'),
    (3, 'ICICI Bank - Savings', 99000, 'E825');

INSERT OR IGNORE INTO BankTransactions
    (Id, TransactionCode, TransactionDate, Description, AccountName, TransactionType, Amount)
VALUES
    (1, 'ETS-26001', '22/10/2026', 'Payment from Sharma Electricals', 'HDFC Bank', 'Credit', 4500),
    (2, 'ETS-26002', '22/10/2026', 'Warehouse utility bill', 'Cash Account', 'Debit', 850),
    (3, 'ETS-26003', '22/10/2026', 'Vendor payment', 'ICICI Bank', 'Debit', 2900),
    (4, 'ETS-26004', '23/10/2026', 'Customer received', 'HDFC Bank', 'Credit', 5200);

INSERT OR IGNORE INTO Vendors
    (Id, SNo, VendorName, CompanyName, PhoneNumber, EmailAddress, OutstandingBalance, Status, Address, AccountName, AccountNumber)
VALUES
    (1, 1, 'Harshit Pandey', 'Harshit01Imports', '555-533-6515', 'harshit@mail.com', 3220, 'Draft', '123 Business Avenue', 'Harshit Pandey', '0000 0000 0000'),
    (2, 2, 'Industrial Solutions Ltd.', 'Industrial Solutions Ltd.', '555-113-7788', 'contact@isl.com', 900, 'Active', 'Warehouse District', 'Industrial Solutions Ltd.', '1000 2300 0044');

INSERT OR IGNORE INTO PurchaseInvoices
    (Id, SNo, PONumber, VendorName, TotalProduct, TotalQuantity, TotalAmount, OrderDate, Status, ProductName, Quantity, Price)
VALUES
    (1, 1, 'PO-2025-000024', 'Harshit Pandey', 5, 155, 3220, '09/05/2026', 'Pending', '9W Light Bulb', 155, 20.77),
    (2, 2, 'PO-2025-000025', 'Industrial Solutions Ltd.', 3, 60, 1250, '10/05/2026', 'Pending', 'Safety Goggles', 60, 20.83);

INSERT OR IGNORE INTO PurchaseReturns
    (Id, SNo, PONumber, VendorName, PurchasedQuantity, ReturnQuantity, ReturnTotal, OrderDate, Status, InvoiceNumber)
VALUES
    (1, 1, 'PO-2025-000024', 'Harshit Pandey', 155, 155, 3220, '09/05/2026', 'Pending', 'INV-2023-0892');

INSERT OR IGNORE INTO VendorPayments
    (Id, SNo, PONumber, VendorName, PaymentStatus, PaidAmount, PendingAmount, TotalAmount, DueDate, LastPaymentDate, PaymentMode)
VALUES
    (1, 1, 'PO-2025-000024', 'Harshit Pandey', 'Paid', 3000, 200, 3220, '12/05/2026', '12/05/2026', 'Bank Transfer');

INSERT OR IGNORE INTO ExpenseSummaryCards (Id, Title, Amount, AccentColor, IconGlyph) VALUES
    (1, 'Total Expenses (This Month)', 99000, '#D8E9FB', 'EAFD'),
    (2, 'Rent', 25000, '#F2DEE0', 'E80F'),
    (3, 'Electricity', 4500, '#F3EFCF', 'E945'),
    (4, 'Salary', 45000, '#D9F2DE', 'E77B');

INSERT OR IGNORE INTO Expenses (Id, ExpenseId, ExpenseDate, Category, Description, Amount, PaidFrom) VALUES
    (1, 'ETS-26001', '22/10/2026', 'Electricity', 'Monthly electricity bill', 4500, 'Cash Account');

INSERT OR IGNORE INTO ExpenseCategoryBreakdown (Id, Category, Percentage) VALUES
    (1, 'Payroll & Salary', 60),
    (2, 'Rent & Office', 25),
    (3, 'Marketing & Sales', 10),
    (4, 'Utilities', 5),
    (5, 'Other', 0);

INSERT OR IGNORE INTO ReportMetricCards (Id, ReportType, Title, DisplayValue, TrendText, AccentColor) VALUES
    (1, 'SalesReport', 'Total Sales', '$ 2,27,980', '+12.5% From Last Period', '#D8E9FB'),
    (2, 'SalesReport', 'Total Invoices', '62', '+8.2% From Last Period', '#F2DEE0'),
    (3, 'SalesReport', 'Items Sold', '740', '+15.3% From Last Period', '#F3EFCF'),
    (4, 'SalesReport', 'Total Profit', '$ 42,350', '+10.8% From Last Period', '#D9F2DE'),
    (5, 'PurchaseReport', 'Total Purchases', '$ 2,27,980', '+12.5% From Last Period', '#D8E9FB'),
    (6, 'PurchaseReport', 'Total Purchase Invoices', '48', '+8.2% From Last Period', '#F2DEE0'),
    (7, 'PurchaseReport', 'Items Purchased', '740', '+15.3% From Last Period', '#F3EFCF'),
    (8, 'PurchaseReport', 'Top Supplier', 'Harshit', '+10.8% From Last Period', '#D9F2DE'),
    (9, 'StockReport', 'Total Products', '186', '+4 New This Period', '#D8E9FB'),
    (10, 'StockReport', 'Total Stock Quantity', '3,420', '+8.5% From Last Period', '#F2DEE0'),
    (11, 'StockReport', 'Stock Value (Cost)', '$ 1 240 500', '+6.3% From Last Period', '#F3EFCF'),
    (12, 'StockReport', 'Stock Value (Selling)', '$ 1 865 200', '+7.9% From Last Period', '#D9F2DE'),
    (13, 'CustomerOutstanding', 'Total Credit Sales', '$ 517 500', '+8.4% From Last Period', '#D8E9FB'),
    (14, 'CustomerOutstanding', 'Total Received', '$ 235 000', '+5.2% From Last Period', '#F2DEE0'),
    (15, 'CustomerOutstanding', 'Total Outstanding', '$ 282 500', '-2.1% From Last Period', '#F3EFCF'),
    (16, 'CustomerOutstanding', 'Overdue Amount', '$ 124 000', '-6.8% From Last Period', '#D9F2DE'),
    (17, 'VendorOutstanding', 'Total Purchases', '$ 1 240 000', '+6.3% From Last Period', '#D8E9FB'),
    (18, 'VendorOutstanding', 'Total Paid To Vendors', '$ 860 000', '+4.8% From Last Period', '#F2DEE0'),
    (19, 'VendorOutstanding', 'Total Outstanding', '$ 282 500', '-3.5% From Last Period', '#F3EFCF'),
    (20, 'VendorOutstanding', 'Overdue Payables', '$ 150 000', '-5.2% From Last Period', '#D9F2DE'),
    (21, 'ProfitAndLoss', 'Total Revenue', '$ 1 240 000', '+6.3% From Last Period', '#D8E9FB'),
    (22, 'ProfitAndLoss', 'Total Cost', '$ 860 000', '+4.8% From Last Period', '#F2DEE0'),
    (23, 'ProfitAndLoss', 'Gross Profit', '$ 282 500', '-3.5% From Last Period', '#F3EFCF'),
    (24, 'ProfitAndLoss', 'Profit Margin', '44.71%', '-5.2% From Last Period', '#D9F2DE');

INSERT OR IGNORE INTO SalesReportRows
    (Id, Date, BillNo, CustomerName, ItemName, Quantity, Rate, Amount, PaymentStatus, PaymentMode, Profit)
VALUES
    (1, '22/10/2026', 'ETS-00123', 'Harshit Pandey', '9w Light Bulb', 10, 300, 3000, 'Paid', 'Cash', 3000);

INSERT OR IGNORE INTO PurchaseReportRows
    (Id, Date, BillNo, VendorName, ItemName, Quantity, PurchasePrice, TotalAmount, PaymentStatus)
VALUES
    (1, '22/10/2026', 'ETS-00123', 'Harshit Pandey', '9w Light Bulb', 10, 300, 3000, 'Paid');

INSERT OR IGNORE INTO StockReportRows
    (Id, ItemName, Category, OpeningStock, PurchaseQuantity, SalesQuantity, CurrentStock, PurchaseValue, SalesValue, ProfitMargin)
VALUES
    (1, '9w Light Bulb', 'Electric', 100, 50, 47, 103, 1.5, 2.5, 1);

INSERT OR IGNORE INTO CustomerOutstandingRows
    (Id, CustomerName, InvoiceNo, InvoiceDate, TotalAmount, PaidAmount, Balance)
VALUES
    (1, 'Harshit Pandey', 'ETS-26001', '17/02/2026', 350, 50, 300);

INSERT OR IGNORE INTO VendorOutstandingRows
    (Id, VendorName, BillNo, BillDate, TotalAmount, PaidAmount, Balance)
VALUES
    (1, 'Harshit Pandey', 'ETS-26001', '17/02/2026', 350, 50, 300);

INSERT OR IGNORE INTO ProfitLossRows
    (Id, Number, Product, UnitsSold, CostPrice, SalesPrice, TotalProfit, MarginPercent)
VALUES
    (1, 1, 'Harshit Pandey', 40, 5, 10, 400, 50);

INSERT OR IGNORE INTO BalanceSheetEntries (Id, EntryType, Section, AccountName, Amount) VALUES
    (1, 'Asset', 'Current Assets', 'Cash in Hand', 25000),
    (2, 'Asset', 'Current Assets', 'HDFC Bank', 85000),
    (3, 'Asset', 'Current Assets', 'Closing Stock', 45000),
    (4, 'Asset', 'Current Assets', 'Customer Outstanding', 28000),
    (5, 'Liability', 'Current Liabilities', 'Vendor Outstanding', 35000),
    (6, 'Liability', 'Current Liabilities', 'Total Liabilities', 35000),
    (7, 'Liability', 'Equity', 'Owner Capital Account', 100000),
    (8, 'Liability', 'Equity', 'Retained Earnings (Profit)', 48000);

INSERT OR IGNORE INTO BalanceSheetTotals (Id, TotalAssets, TotalLiabilitiesAndEquity) VALUES
    (1, 183000, 183000);

INSERT OR IGNORE INTO CompanyProfileSettings
    (Id, CompanyName, MobileNumber, EmailAddress, Address, LogoPath)
VALUES
    (1, 'Acme Global Solutions Inc.', '+1 (212) 555-0198', 'finance@acme-global.com', '1234 Enterprise Way, Suite 500
New York, NY 10001
United States', '');

INSERT OR IGNORE INTO LedgerAccounts
    (Id, AccountName, AccountType, OpeningBalance, CreatedDate)
VALUES
    (1, 'Cash Account', 'Cash', 45000, 'Jan 12, 2024'),
    (2, 'Bank Account', 'Bank', 820000, 'Jan 12, 2024'),
    (3, 'Owner Capital', 'Capital', 1000000, 'Jan 15, 2024'),
    (4, 'Sales Account', 'Sales', 0, 'Feb 01, 2024'),
    (5, 'Purchase Account', 'Purchase', 0, 'Feb 01, 2024'),
    (6, 'Expense Account', 'Expense', 15400, 'Feb 05, 2024');

INSERT OR IGNORE INTO AppUsers
    (Id, UserName, EmailAddress, MobileNumber, Role, IsActive, PasswordHash)
VALUES
    (1, 'Alex Johnson', 'alex.j@enterprise.com', '+1 (555) 123-4567', 'Owner', 1, 'hash-owner'),
    (2, 'Sarah Miller', 's.miller@enterprise.com', '+1 (555) 987-6543', 'Accountant', 1, 'hash-accountant'),
    (3, 'Marcus Chen', 'm.chen@enterprise.com', '+1 (555) 444-2211', 'Billing Operator', 0, 'hash-billing'),
    (4, 'David Wright', 'david.w@enterprise.com', '+1 (555) 777-3322', 'Accountant', 1, 'hash-accountant-2');

INSERT OR IGNORE INTO BackupStatus
    (Id, CurrentStatus, LastSuccessfulBackup, NextScheduledBackup, AutoBackupEnabled)
VALUES
    (1, 'System Healthy', '17 Feb 2026, 10:30 AM', 'Today, 00:00 UTC', 1);

INSERT OR IGNORE INTO BackupLogs
    (Id, Activity, DateAndTime, Size, Status)
VALUES
    (1, 'Automated Daily Backup', '17 Feb 2026, 00:00 AM', '42.5 MB', 'Success'),
    (2, 'Manual Backup (Admin)', '16 Feb 2026, 04:15 PM', '41.8 MB', 'Success'),
    (3, 'Automated Daily Backup', '16 Feb 2026, 00:00 AM', '--', 'Failed'),
    (4, 'Automated Daily Backup', '15 Feb 2026, 00:00 AM', '41.2 MB', 'Success');
