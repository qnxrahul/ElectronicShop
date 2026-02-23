using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class BillingInvoicesViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private string _selectedTab = "Invoices";
    private string _searchText = string.Empty;

    public BillingInvoicesViewModel(AccountingRepository repository)
    {
        _repository = repository;
        SelectInvoicesCommand = new RelayCommand(() => SelectedTab = "Invoices");
        SelectReturnsCommand = new RelayCommand(() => SelectedTab = "Return & Exchange");
        SelectCompletedCommand = new RelayCommand(() => SelectedTab = "Completed");
        LoadData();
    }

    public ObservableCollection<BillingInvoiceRow> Invoices { get; } = [];

    public string SelectedTab
    {
        get => _selectedTab;
        set => SetProperty(ref _selectedTab, value);
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public RelayCommand SelectInvoicesCommand { get; }

    public RelayCommand SelectReturnsCommand { get; }

    public RelayCommand SelectCompletedCommand { get; }

    private void LoadData()
    {
        Invoices.Clear();
        foreach (var invoice in _repository.GetBillingInvoices())
        {
            Invoices.Add(invoice);
        }
    }
}
