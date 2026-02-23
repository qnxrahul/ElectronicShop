using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class InventoryViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private string _searchText = string.Empty;

    public InventoryViewModel(AccountingRepository repository)
    {
        _repository = repository;
        LoadData();
    }

    public ObservableCollection<InventoryOverviewCard> OverviewCards { get; } = [];

    public ObservableCollection<InventoryItemRow> InventoryItems { get; } = [];

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    private void LoadData()
    {
        OverviewCards.Clear();
        foreach (var card in _repository.GetInventoryOverviewCards())
        {
            OverviewCards.Add(card);
        }

        InventoryItems.Clear();
        foreach (var item in _repository.GetInventoryItems())
        {
            InventoryItems.Add(item);
        }
    }
}
