using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class ReportsViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private ReportTab _selectedTab = ReportTab.SalesReport;

    public ReportsViewModel(AccountingRepository repository)
    {
        _repository = repository;

        SelectSalesReportCommand = new RelayCommand(() => SelectedTab = ReportTab.SalesReport);
        SelectPurchaseReportCommand = new RelayCommand(() => SelectedTab = ReportTab.PurchaseReport);
        SelectStockReportCommand = new RelayCommand(() => SelectedTab = ReportTab.StockReport);
        SelectCustomerOutstandingCommand = new RelayCommand(() => SelectedTab = ReportTab.CustomerOutstanding);
        SelectVendorOutstandingCommand = new RelayCommand(() => SelectedTab = ReportTab.VendorOutstanding);
        SelectProfitLossCommand = new RelayCommand(() => SelectedTab = ReportTab.ProfitAndLoss);
        SelectBalanceSheetCommand = new RelayCommand(() => SelectedTab = ReportTab.BalanceSheet);

        LoadData();
    }

    public ObservableCollection<ReportMetricCard> MetricCards { get; } = [];

    public ObservableCollection<SalesReportRow> SalesRows { get; } = [];

    public ObservableCollection<PurchaseReportRow> PurchaseRows { get; } = [];

    public ObservableCollection<StockReportRow> StockRows { get; } = [];

    public ObservableCollection<CustomerOutstandingRow> CustomerOutstandingRows { get; } = [];

    public ObservableCollection<VendorOutstandingRow> VendorOutstandingRows { get; } = [];

    public ObservableCollection<ProfitLossRow> ProfitLossRows { get; } = [];

    public ObservableCollection<BalanceSheetEntry> AssetEntries { get; } = [];

    public ObservableCollection<BalanceSheetEntry> LiabilityEntries { get; } = [];

    public BalanceSheetTotals BalanceSheetTotals { get; private set; } = new();

    public ReportTab SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (!SetProperty(ref _selectedTab, value))
            {
                return;
            }

            LoadMetricCards();
            OnPropertyChanged(nameof(IsSalesVisible));
            OnPropertyChanged(nameof(IsPurchaseVisible));
            OnPropertyChanged(nameof(IsStockVisible));
            OnPropertyChanged(nameof(IsCustomerOutstandingVisible));
            OnPropertyChanged(nameof(IsVendorOutstandingVisible));
            OnPropertyChanged(nameof(IsProfitLossVisible));
            OnPropertyChanged(nameof(IsBalanceSheetVisible));
            OnPropertyChanged(nameof(CurrentReportTitle));
        }
    }

    public bool IsSalesVisible => SelectedTab == ReportTab.SalesReport;

    public bool IsPurchaseVisible => SelectedTab == ReportTab.PurchaseReport;

    public bool IsStockVisible => SelectedTab == ReportTab.StockReport;

    public bool IsCustomerOutstandingVisible => SelectedTab == ReportTab.CustomerOutstanding;

    public bool IsVendorOutstandingVisible => SelectedTab == ReportTab.VendorOutstanding;

    public bool IsProfitLossVisible => SelectedTab == ReportTab.ProfitAndLoss;

    public bool IsBalanceSheetVisible => SelectedTab == ReportTab.BalanceSheet;

    public string CurrentReportTitle => SelectedTab switch
    {
        ReportTab.SalesReport => "Sales Report",
        ReportTab.PurchaseReport => "Purchase Report",
        ReportTab.StockReport => "Stock Report",
        ReportTab.CustomerOutstanding => "Customer Outstanding",
        ReportTab.VendorOutstanding => "Vendor Outstanding",
        ReportTab.ProfitAndLoss => "Profit & Loss",
        ReportTab.BalanceSheet => "Balance Sheet",
        _ => "Report"
    };

    public RelayCommand SelectSalesReportCommand { get; }

    public RelayCommand SelectPurchaseReportCommand { get; }

    public RelayCommand SelectStockReportCommand { get; }

    public RelayCommand SelectCustomerOutstandingCommand { get; }

    public RelayCommand SelectVendorOutstandingCommand { get; }

    public RelayCommand SelectProfitLossCommand { get; }

    public RelayCommand SelectBalanceSheetCommand { get; }

    private void LoadData()
    {
        ReplaceCollection(SalesRows, _repository.GetSalesReportRows());
        ReplaceCollection(PurchaseRows, _repository.GetPurchaseReportRows());
        ReplaceCollection(StockRows, _repository.GetStockReportRows());
        ReplaceCollection(CustomerOutstandingRows, _repository.GetCustomerOutstandingRows());
        ReplaceCollection(VendorOutstandingRows, _repository.GetVendorOutstandingRows());
        ReplaceCollection(ProfitLossRows, _repository.GetProfitLossRows());
        ReplaceCollection(AssetEntries, _repository.GetBalanceSheetEntries("Asset"));
        ReplaceCollection(LiabilityEntries, _repository.GetBalanceSheetEntries("Liability"));
        BalanceSheetTotals = _repository.GetBalanceSheetTotals();
        OnPropertyChanged(nameof(BalanceSheetTotals));
        LoadMetricCards();
    }

    private void LoadMetricCards()
    {
        ReplaceCollection(MetricCards, _repository.GetReportMetricCards(SelectedTab));
    }

    private static void ReplaceCollection<T>(ICollection<T> target, IEnumerable<T> source)
    {
        target.Clear();
        foreach (var item in source)
        {
            target.Add(item);
        }
    }
}
