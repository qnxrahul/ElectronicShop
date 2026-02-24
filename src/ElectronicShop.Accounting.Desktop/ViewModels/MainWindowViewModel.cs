using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly DashboardViewModel _dashboardViewModel;
    private readonly BillingInvoicesViewModel _billingViewModel;
    private readonly InventoryViewModel _inventoryViewModel;
    private readonly VendorsPurchasesViewModel _vendorsPurchasesViewModel;
    private readonly BankingViewModel _bankingViewModel;
    private readonly ExpensesViewModel _expensesViewModel;
    private readonly ReportsViewModel _reportsViewModel;
    private readonly SettingsViewModel _settingsViewModel;
    private ViewModelBase _currentPage;
    private NavigationItem? _selectedNavigationItem;
    private string _globalSearchText = string.Empty;

    public MainWindowViewModel(AccountingRepository repository)
    {
        _dashboardViewModel = new DashboardViewModel(repository);
        _billingViewModel = new BillingInvoicesViewModel(repository);
        _inventoryViewModel = new InventoryViewModel(repository);
        _vendorsPurchasesViewModel = new VendorsPurchasesViewModel(repository);
        _bankingViewModel = new BankingViewModel(repository);
        _expensesViewModel = new ExpensesViewModel(repository);
        _reportsViewModel = new ReportsViewModel(repository);
        _settingsViewModel = new SettingsViewModel(repository);

        NavigationItems =
        [
            new NavigationItem(NavigationTarget.Dashboard, "Dashboard", "\uE80F"),
            new NavigationItem(NavigationTarget.Billing, "Billing & Invoice", "\uE8A5"),
            new NavigationItem(NavigationTarget.Inventory, "Inventory & Services", "\uE14C"),
            new NavigationItem(NavigationTarget.Vendors, "Vendors & Purchases", "\uED44"),
            new NavigationItem(NavigationTarget.Banking, "Banking", "\uE825"),
            new NavigationItem(NavigationTarget.Expenses, "Expenses", "\uEAFD"),
            new NavigationItem(NavigationTarget.Reports, "Reports", "\uE9D2"),
            new NavigationItem(NavigationTarget.Settings, "Settings", "\uE713")
        ];

        _currentPage = _dashboardViewModel;
        SelectedNavigationItem = NavigationItems[0];
    }

    public ObservableCollection<NavigationItem> NavigationItems { get; }

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        private set => SetProperty(ref _currentPage, value);
    }

    public NavigationItem? SelectedNavigationItem
    {
        get => _selectedNavigationItem;
        set
        {
            if (!SetProperty(ref _selectedNavigationItem, value) || value is null)
            {
                return;
            }

            Navigate(value.Target);
        }
    }

    public string GlobalSearchText
    {
        get => _globalSearchText;
        set => SetProperty(ref _globalSearchText, value);
    }

    private void Navigate(NavigationTarget target)
    {
        switch (target)
        {
            case NavigationTarget.Dashboard:
                CurrentPage = _dashboardViewModel;
                break;
            case NavigationTarget.Billing:
                CurrentPage = _billingViewModel;
                break;
            case NavigationTarget.Inventory:
                CurrentPage = _inventoryViewModel;
                break;
            case NavigationTarget.Vendors:
                CurrentPage = _vendorsPurchasesViewModel;
                break;
            case NavigationTarget.Banking:
                CurrentPage = _bankingViewModel;
                break;
            case NavigationTarget.Expenses:
                CurrentPage = _expensesViewModel;
                break;
            case NavigationTarget.Reports:
                CurrentPage = _reportsViewModel;
                break;
            case NavigationTarget.Settings:
                CurrentPage = _settingsViewModel;
                break;
            default:
                CurrentPage = _dashboardViewModel;
                break;
        }
    }
}
