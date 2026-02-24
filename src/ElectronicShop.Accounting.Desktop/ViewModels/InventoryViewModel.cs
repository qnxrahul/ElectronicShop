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
    private string _newCategory = "LED & Lighting";
    private string _newProductCode = string.Empty;
    private decimal _newPurchasePrice;
    private decimal _newSalesPrice;
    private int _newOpeningStock;
    private int _newReorderLevel = 20;

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
        _repository.AddInventoryItem(
            new AddInventoryItemInput
            {
                ProductName = string.IsNullOrWhiteSpace(NewProductName) ? "New Product" : NewProductName,
                Category = string.IsNullOrWhiteSpace(NewCategory) ? "General" : NewCategory,
                ProductCode = NewProductCode,
                PurchasePrice = NewPurchasePrice <= 0 ? 100m : NewPurchasePrice,
                SalesPrice = NewSalesPrice <= 0 ? 120m : NewSalesPrice,
                OpeningStock = NewOpeningStock < 0 ? 0 : NewOpeningStock,
                ReorderLevel = NewReorderLevel <= 0 ? 10 : NewReorderLevel
            });

        IsAddProductOpen = false;
        NewProductName = string.Empty;
        NewProductCode = string.Empty;
        NewPurchasePrice = 0m;
        NewSalesPrice = 0m;
        NewOpeningStock = 0;
        NewReorderLevel = 20;
        LoadData();
    }
}
