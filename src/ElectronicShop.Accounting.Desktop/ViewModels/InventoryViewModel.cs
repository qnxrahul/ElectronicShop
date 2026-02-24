using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class InventoryViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private string _searchText = string.Empty;
    private bool _isAddProductOpen;
    private string _newProductName = string.Empty;
    private string _newCategory = string.Empty;
    private string _newProductCode = string.Empty;
    private string _newHsn = string.Empty;
    private decimal _newPurchasePrice;
    private decimal _newSalesPrice;
    private int _newOpeningStock;
    private int _newReorderLevel;

    public InventoryViewModel(AccountingRepository repository)
    {
        _repository = repository;
        OpenAddProductCommand = new RelayCommand(() => IsAddProductOpen = true);
        CloseAddProductCommand = new RelayCommand(() => IsAddProductOpen = false);
        SaveProductCommand = new RelayCommand(SaveProduct);
        LoadData();
    }

    public ObservableCollection<InventoryOverviewCard> OverviewCards { get; } = [];

    public ObservableCollection<InventoryItemRow> InventoryItems { get; } = [];

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsAddProductOpen
    {
        get => _isAddProductOpen;
        set => SetProperty(ref _isAddProductOpen, value);
    }

    public string NewProductName
    {
        get => _newProductName;
        set => SetProperty(ref _newProductName, value);
    }

    public string NewCategory
    {
        get => _newCategory;
        set => SetProperty(ref _newCategory, value);
    }

    public string NewProductCode
    {
        get => _newProductCode;
        set => SetProperty(ref _newProductCode, value);
    }

    public string NewHsn
    {
        get => _newHsn;
        set => SetProperty(ref _newHsn, value);
    }

    public decimal NewPurchasePrice
    {
        get => _newPurchasePrice;
        set => SetProperty(ref _newPurchasePrice, value);
    }

    public decimal NewSalesPrice
    {
        get => _newSalesPrice;
        set => SetProperty(ref _newSalesPrice, value);
    }

    public int NewOpeningStock
    {
        get => _newOpeningStock;
        set => SetProperty(ref _newOpeningStock, value);
    }

    public int NewReorderLevel
    {
        get => _newReorderLevel;
        set => SetProperty(ref _newReorderLevel, value);
    }

    public RelayCommand OpenAddProductCommand { get; }

    public RelayCommand CloseAddProductCommand { get; }

    public RelayCommand SaveProductCommand { get; }

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

    private void SaveProduct()
    {
        if (string.IsNullOrWhiteSpace(NewProductName) ||
            string.IsNullOrWhiteSpace(NewCategory) ||
            string.IsNullOrWhiteSpace(NewProductCode) ||
            string.IsNullOrWhiteSpace(NewHsn) ||
            NewPurchasePrice <= 0 ||
            NewSalesPrice <= 0 ||
            NewOpeningStock < 0 ||
            NewReorderLevel <= 0)
        {
            return;
        }

        _repository.AddInventoryItem(
            new AddInventoryItemInput
            {
                ProductName = NewProductName,
                Category = NewCategory,
                ProductCode = NewProductCode,
                Hsn = NewHsn,
                PurchasePrice = NewPurchasePrice,
                SalesPrice = NewSalesPrice,
                OpeningStock = NewOpeningStock,
                ReorderLevel = NewReorderLevel
            });

        IsAddProductOpen = false;
        NewProductName = string.Empty;
        NewCategory = string.Empty;
        NewProductCode = string.Empty;
        NewHsn = string.Empty;
        NewPurchasePrice = 0m;
        NewSalesPrice = 0m;
        NewOpeningStock = 0;
        NewReorderLevel = 0;
        LoadData();
    }
}
