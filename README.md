# ElectronicShop

Accounting desktop implementation (WPF + MVVM + SQLite + Dapper) is now scaffolded and expanded with full module coverage.

- WPF + MVVM architecture
- SQLite local database
- Dapper-based data access
- .NET 8 targeting Windows 10 and above

## Project structure

- `ElectronicShop.Accounting.sln`
- `src/ElectronicShop.Accounting.Desktop/`
  - `Views/` (Dashboard, Billing & Invoice, Inventory & Services, Banking)
  - `ViewModels/` (MVVM bindings and screen state)
  - `Data/` (SQLite schema + seed data + Dapper repository)
  - `Models/`, `Infrastructure/`, `Styles/`

## Screens implemented

Navigation modules implemented:

1. Dashboard
2. Billing & Invoice (including create invoice dialogs)
3. Inventory & Services (including add product dialog)
4. Vendors & Purchases (Vendors, Purchase Invoice, Purchase Return, Pay Bills + dialogs)
5. Banking (including new transaction dialog)
6. Expenses (including add expense dialog)
7. Reports (Sales, Purchase, Stock, Customer Outstanding, Vendor Outstanding, Profit & Loss, Balance Sheet)
8. Settings (Company Profile, Accounts Management, User Management, Backup & Security + add dialogs)

All modules are data-backed through SQLite + Dapper seed/repository methods. Create flows from dialogs are wired to database insert/update operations.

## Running locally (Windows)

1. Open `ElectronicShop.Accounting.sln` in Visual Studio 2022+.
2. Restore NuGet packages.
3. Build and run `ElectronicShop.Accounting.Desktop`.

On first launch, the app initializes and seeds a SQLite database under:

`%LocalAppData%\ElectronicShop\Accounting\accounting.db`