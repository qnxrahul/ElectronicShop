using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class VendorsPurchasesViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private string _selectedTab = "Vendors";
    private string _searchText = string.Empty;
    private bool _isAddVendorOpen;
    private bool _isPurchaseInvoiceOpen;
    private bool _isPurchaseReturnOpen;
    private bool _isPayBillOpen;

    private string _vendorNameInput = string.Empty;
    private string _vendorCompanyInput = string.Empty;
    private string _vendorEmailInput = string.Empty;
    private string _vendorPhoneInput = string.Empty;
    private string _vendorAddressInput = string.Empty;
    private string _vendorAccountNameInput = string.Empty;
    private string _vendorAccountNumberInput = string.Empty;

    private string _purchaseVendorNameInput = "Harshit Pandey";
    private string _purchaseDateInput = DateTime.Now.ToString("MM/dd/yyyy");
    private string _purchaseStatusInput = "Pending";
    private string _purchaseProductInput = "9W Light Bulb";
    private int _purchaseQuantityInput = 2;
    private decimal _purchasePriceInput = 2000m;

    private string _returnVendorNameInput = "Industrial Solutions Ltd.";
    private string _returnInvoiceInput = "INV-2023-0892";
    private string _returnDateInput = DateTime.Now.ToString("MM/dd/yyyy");
    private int _returnQuantityInput = 2;
    private decimal _returnUnitPriceInput = 450m;

    private string _payBillVendorNameInput = "Harshit Pandey";
    private string _payBillInvoiceInput = "PO-2025-000024";
    private string _payBillDateInput = DateTime.Now.ToString("MM/dd/yyyy");
    private string _payBillModeInput = "Bank Transfer";
    private decimal _payBillAmountInput = 1000m;

    public VendorsPurchasesViewModel(AccountingRepository repository)
    {
        _repository = repository;
        SelectVendorsCommand = new RelayCommand(() => SelectedTab = "Vendors");
        SelectPurchaseInvoiceCommand = new RelayCommand(() => SelectedTab = "Purchase Invoice");
        SelectPurchaseReturnCommand = new RelayCommand(() => SelectedTab = "Purchase Return");
        SelectPayBillsCommand = new RelayCommand(() => SelectedTab = "Pay Bills");

        OpenPrimaryActionCommand = new RelayCommand(OpenPrimaryAction);
        CloseAddVendorCommand = new RelayCommand(() => IsAddVendorOpen = false);
        ClosePurchaseInvoiceCommand = new RelayCommand(() => IsPurchaseInvoiceOpen = false);
        ClosePurchaseReturnCommand = new RelayCommand(() => IsPurchaseReturnOpen = false);
        ClosePayBillCommand = new RelayCommand(() => IsPayBillOpen = false);

        SaveVendorCommand = new RelayCommand(SaveVendor);
        SavePurchaseInvoiceCommand = new RelayCommand(SavePurchaseInvoice);
        SavePurchaseReturnCommand = new RelayCommand(SavePurchaseReturn);
        SavePayBillCommand = new RelayCommand(SavePayBill);

        LoadData();
    }

    public ObservableCollection<VendorRow> Vendors { get; } = [];

    public ObservableCollection<PurchaseInvoiceRow> PurchaseInvoices { get; } = [];

    public ObservableCollection<PurchaseReturnRow> PurchaseReturns { get; } = [];

    public ObservableCollection<VendorPayBillRow> PayBills { get; } = [];

    public string SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (!SetProperty(ref _selectedTab, value))
            {
                return;
            }

            LoadData();
            OnPropertyChanged(nameof(PrimaryActionText));
            OnPropertyChanged(nameof(PageTitle));
        }
    }

    public string PageTitle => SelectedTab == "Purchase Return" ? "Purchases Return" : "Vendors & Purchases";

    public string PrimaryActionText => SelectedTab switch
    {
        "Purchase Invoice" => "New Purchase Invoice",
        "Purchase Return" => "Purchase Return Invoice",
        "Pay Bills" => "Pay Bill",
        _ => "Add Vendor"
    };

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsAddVendorOpen
    {
        get => _isAddVendorOpen;
        set => SetProperty(ref _isAddVendorOpen, value);
    }

    public bool IsPurchaseInvoiceOpen
    {
        get => _isPurchaseInvoiceOpen;
        set => SetProperty(ref _isPurchaseInvoiceOpen, value);
    }

    public bool IsPurchaseReturnOpen
    {
        get => _isPurchaseReturnOpen;
        set => SetProperty(ref _isPurchaseReturnOpen, value);
    }

    public bool IsPayBillOpen
    {
        get => _isPayBillOpen;
        set => SetProperty(ref _isPayBillOpen, value);
    }

    public string VendorNameInput
    {
        get => _vendorNameInput;
        set => SetProperty(ref _vendorNameInput, value);
    }

    public string VendorCompanyInput
    {
        get => _vendorCompanyInput;
        set => SetProperty(ref _vendorCompanyInput, value);
    }

    public string VendorEmailInput
    {
        get => _vendorEmailInput;
        set => SetProperty(ref _vendorEmailInput, value);
    }

    public string VendorPhoneInput
    {
        get => _vendorPhoneInput;
        set => SetProperty(ref _vendorPhoneInput, value);
    }

    public string VendorAddressInput
    {
        get => _vendorAddressInput;
        set => SetProperty(ref _vendorAddressInput, value);
    }

    public string VendorAccountNameInput
    {
        get => _vendorAccountNameInput;
        set => SetProperty(ref _vendorAccountNameInput, value);
    }

    public string VendorAccountNumberInput
    {
        get => _vendorAccountNumberInput;
        set => SetProperty(ref _vendorAccountNumberInput, value);
    }

    public string PurchaseVendorNameInput
    {
        get => _purchaseVendorNameInput;
        set => SetProperty(ref _purchaseVendorNameInput, value);
    }

    public string PurchaseDateInput
    {
        get => _purchaseDateInput;
        set => SetProperty(ref _purchaseDateInput, value);
    }

    public string PurchaseStatusInput
    {
        get => _purchaseStatusInput;
        set => SetProperty(ref _purchaseStatusInput, value);
    }

    public string PurchaseProductInput
    {
        get => _purchaseProductInput;
        set => SetProperty(ref _purchaseProductInput, value);
    }

    public int PurchaseQuantityInput
    {
        get => _purchaseQuantityInput;
        set
        {
            if (SetProperty(ref _purchaseQuantityInput, value))
            {
                OnPropertyChanged(nameof(PurchaseInvoiceTotal));
            }
        }
    }

    public decimal PurchasePriceInput
    {
        get => _purchasePriceInput;
        set
        {
            if (SetProperty(ref _purchasePriceInput, value))
            {
                OnPropertyChanged(nameof(PurchaseInvoiceTotal));
            }
        }
    }

    public string ReturnVendorNameInput
    {
        get => _returnVendorNameInput;
        set => SetProperty(ref _returnVendorNameInput, value);
    }

    public string ReturnInvoiceInput
    {
        get => _returnInvoiceInput;
        set => SetProperty(ref _returnInvoiceInput, value);
    }

    public string ReturnDateInput
    {
        get => _returnDateInput;
        set => SetProperty(ref _returnDateInput, value);
    }

    public int ReturnQuantityInput
    {
        get => _returnQuantityInput;
        set
        {
            if (SetProperty(ref _returnQuantityInput, value))
            {
                OnPropertyChanged(nameof(PurchaseReturnTotal));
            }
        }
    }

    public decimal ReturnUnitPriceInput
    {
        get => _returnUnitPriceInput;
        set
        {
            if (SetProperty(ref _returnUnitPriceInput, value))
            {
                OnPropertyChanged(nameof(PurchaseReturnTotal));
            }
        }
    }

    public string PayBillVendorNameInput
    {
        get => _payBillVendorNameInput;
        set => SetProperty(ref _payBillVendorNameInput, value);
    }

    public string PayBillInvoiceInput
    {
        get => _payBillInvoiceInput;
        set => SetProperty(ref _payBillInvoiceInput, value);
    }

    public string PayBillDateInput
    {
        get => _payBillDateInput;
        set => SetProperty(ref _payBillDateInput, value);
    }

    public string PayBillModeInput
    {
        get => _payBillModeInput;
        set => SetProperty(ref _payBillModeInput, value);
    }

    public decimal PayBillAmountInput
    {
        get => _payBillAmountInput;
        set => SetProperty(ref _payBillAmountInput, value);
    }

    public decimal PurchaseInvoiceTotal => PurchaseQuantityInput * PurchasePriceInput;

    public decimal PurchaseReturnTotal => ReturnQuantityInput * ReturnUnitPriceInput;

    public RelayCommand SelectVendorsCommand { get; }

    public RelayCommand SelectPurchaseInvoiceCommand { get; }

    public RelayCommand SelectPurchaseReturnCommand { get; }

    public RelayCommand SelectPayBillsCommand { get; }

    public RelayCommand OpenPrimaryActionCommand { get; }

    public RelayCommand CloseAddVendorCommand { get; }

    public RelayCommand ClosePurchaseInvoiceCommand { get; }

    public RelayCommand ClosePurchaseReturnCommand { get; }

    public RelayCommand ClosePayBillCommand { get; }

    public RelayCommand SaveVendorCommand { get; }

    public RelayCommand SavePurchaseInvoiceCommand { get; }

    public RelayCommand SavePurchaseReturnCommand { get; }

    public RelayCommand SavePayBillCommand { get; }

    private void LoadData()
    {
        ReplaceCollection(Vendors, _repository.GetVendors());
        ReplaceCollection(PurchaseInvoices, _repository.GetPurchaseInvoices());
        ReplaceCollection(PurchaseReturns, _repository.GetPurchaseReturns());
        ReplaceCollection(PayBills, _repository.GetVendorPayments());
    }

    private void OpenPrimaryAction()
    {
        switch (SelectedTab)
        {
            case "Purchase Invoice":
                IsPurchaseInvoiceOpen = true;
                break;
            case "Purchase Return":
                IsPurchaseReturnOpen = true;
                break;
            case "Pay Bills":
                IsPayBillOpen = true;
                break;
            default:
                IsAddVendorOpen = true;
                break;
        }
    }

    private void SaveVendor()
    {
        _repository.AddVendor(
            new AddVendorInput
            {
                VendorName = string.IsNullOrWhiteSpace(VendorNameInput) ? "New Vendor" : VendorNameInput,
                CompanyName = string.IsNullOrWhiteSpace(VendorCompanyInput) ? VendorNameInput : VendorCompanyInput,
                EmailAddress = string.IsNullOrWhiteSpace(VendorEmailInput) ? "contact@vendor.com" : VendorEmailInput,
                PhoneNumber = string.IsNullOrWhiteSpace(VendorPhoneInput) ? "000-000-0000" : VendorPhoneInput,
                Address = string.IsNullOrWhiteSpace(VendorAddressInput) ? "Unknown address" : VendorAddressInput,
                AccountName = string.IsNullOrWhiteSpace(VendorAccountNameInput) ? "Vendor Account" : VendorAccountNameInput,
                AccountNumber = string.IsNullOrWhiteSpace(VendorAccountNumberInput) ? "0000 0000 0000" : VendorAccountNumberInput
            });

        IsAddVendorOpen = false;
        VendorNameInput = string.Empty;
        VendorCompanyInput = string.Empty;
        VendorEmailInput = string.Empty;
        VendorPhoneInput = string.Empty;
        VendorAddressInput = string.Empty;
        VendorAccountNameInput = string.Empty;
        VendorAccountNumberInput = string.Empty;
        LoadData();
    }

    private void SavePurchaseInvoice()
    {
        _repository.AddPurchaseInvoice(
            new AddPurchaseInvoiceInput
            {
                VendorName = string.IsNullOrWhiteSpace(PurchaseVendorNameInput) ? "Harshit Pandey" : PurchaseVendorNameInput,
                PurchaseDate = string.IsNullOrWhiteSpace(PurchaseDateInput) ? DateTime.Now.ToString("MM/dd/yyyy") : PurchaseDateInput,
                PurchaseStatus = string.IsNullOrWhiteSpace(PurchaseStatusInput) ? "Pending" : PurchaseStatusInput,
                ProductName = string.IsNullOrWhiteSpace(PurchaseProductInput) ? "General Product" : PurchaseProductInput,
                Quantity = PurchaseQuantityInput <= 0 ? 1 : PurchaseQuantityInput,
                Price = PurchasePriceInput <= 0 ? 100m : PurchasePriceInput
            });

        IsPurchaseInvoiceOpen = false;
        LoadData();
    }

    private void SavePurchaseReturn()
    {
        _repository.AddPurchaseReturn(
            new AddPurchaseReturnInput
            {
                VendorName = string.IsNullOrWhiteSpace(ReturnVendorNameInput) ? "Industrial Solutions Ltd." : ReturnVendorNameInput,
                InvoiceNumber = string.IsNullOrWhiteSpace(ReturnInvoiceInput) ? "INV-NEW" : ReturnInvoiceInput,
                ReturnDate = string.IsNullOrWhiteSpace(ReturnDateInput) ? DateTime.Now.ToString("MM/dd/yyyy") : ReturnDateInput,
                ReturnQuantity = ReturnQuantityInput <= 0 ? 1 : ReturnQuantityInput,
                UnitPrice = ReturnUnitPriceInput <= 0 ? 100m : ReturnUnitPriceInput
            });

        IsPurchaseReturnOpen = false;
        LoadData();
    }

    private void SavePayBill()
    {
        _repository.AddVendorPayment(
            new AddVendorPaymentInput
            {
                VendorName = string.IsNullOrWhiteSpace(PayBillVendorNameInput) ? "Harshit Pandey" : PayBillVendorNameInput,
                InvoiceNumber = string.IsNullOrWhiteSpace(PayBillInvoiceInput) ? "PO-NEW" : PayBillInvoiceInput,
                PaymentDate = string.IsNullOrWhiteSpace(PayBillDateInput) ? DateTime.Now.ToString("MM/dd/yyyy") : PayBillDateInput,
                Amount = PayBillAmountInput <= 0 ? 100m : PayBillAmountInput,
                PaymentMode = string.IsNullOrWhiteSpace(PayBillModeInput) ? "Bank Transfer" : PayBillModeInput
            });

        IsPayBillOpen = false;
        LoadData();
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
