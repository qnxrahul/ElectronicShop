using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class DashboardViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;

    public DashboardViewModel(AccountingRepository repository)
    {
        _repository = repository;
        RefreshCommand = new RelayCommand(LoadData);
        LoadData();
    }

    public ObservableCollection<DashboardSummaryCard> SummaryCards { get; } = [];

    public ObservableCollection<MonthlySalesPoint> MonthlySales { get; } = [];

    public ObservableCollection<TopSellingItem> TopSellingItems { get; } = [];

    public ObservableCollection<RecentInvoice> RecentInvoices { get; } = [];

    public ObservableCollection<BranchRevenueItem> BranchRevenueItems { get; } = [];

    public RelayCommand RefreshCommand { get; }

    private void LoadData()
    {
        ReplaceCollection(SummaryCards, _repository.GetDashboardSummaryCards());

        var monthly = _repository.GetMonthlySalesTrend().ToList();
        var maxAmount = monthly.Count == 0 ? 1m : monthly.Max(item => item.Amount);
        foreach (var point in monthly)
        {
            point.BarHeight = 20 + (double)(point.Amount / maxAmount * 145m);
        }

        ReplaceCollection(MonthlySales, monthly);
        ReplaceCollection(TopSellingItems, _repository.GetTopSellingItems());
        ReplaceCollection(RecentInvoices, _repository.GetRecentInvoices());
        ReplaceCollection(BranchRevenueItems, _repository.GetBranchRevenueItems());
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
