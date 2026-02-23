# ElectronicShop

Initial implementation for the accounting desktop application is now scaffolded with:

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

## Screens implemented in this iteration

1. Dashboard
2. Billing & Invoice
3. Inventory & Services
4. Banking

Sidebar entries for Vendors, Expenses, Reports, and Settings are wired and currently show a placeholder screen for the next iteration.

## Running locally (Windows)

1. Open `ElectronicShop.Accounting.sln` in Visual Studio 2022+.
2. Restore NuGet packages.
3. Build and run `ElectronicShop.Accounting.Desktop`.

On first launch, the app initializes and seeds a SQLite database under:

`%LocalAppData%\ElectronicShop\Accounting\accounting.db`