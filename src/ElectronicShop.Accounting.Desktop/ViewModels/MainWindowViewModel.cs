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
    private readonly BankingViewModel _bankingViewModel;
    private readonly PlaceholderViewModel _placeholderViewModel;
    private ViewModelBase _currentPage;
    private NavigationItem? _selectedNavigationItem;
    private string _globalSearchText = string.Empty;

    public MainWindowViewModel(AccountingRepository repository)
    {
        _dashboardViewModel = new DashboardViewModel(repository);
        _billingViewModel = new BillingInvoicesViewModel(repository);
        _inventoryViewModel = new InventoryViewModel(repository);
        _bankingViewModel = new BankingViewModel(repository);
        _placeholderViewModel = new PlaceholderViewModel();

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
            case NavigationTarget.Banking:
                CurrentPage = _bankingViewModel;
                break;
            case NavigationTarget.Vendors:
                ShowPlaceholder("Vendors & Purchases", "Vendor management and purchase flows will be added next.");
                break;
            case NavigationTarget.Expenses:
                ShowPlaceholder("Expenses", "Expense categories and journal workflows are planned for the next pass.");
                break;
            case NavigationTarget.Reports:
                ShowPlaceholder("Reports", "Advanced reporting and export features are planned for the next pass.");
                break;
            case NavigationTarget.Settings:
                ShowPlaceholder("Settings", "Company profile and app settings are planned for the next pass.");
                break;
            default:
                ShowPlaceholder("Coming Soon", "This module is not implemented yet.");
                break;
        }
    }

    private void ShowPlaceholder(string title, string description)
    {
        _placeholderViewModel.Title = title;
        _placeholderViewModel.Description = description;
        CurrentPage = _placeholderViewModel;
    }
}
