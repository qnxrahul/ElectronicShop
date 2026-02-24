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

    private string _purchaseVendorNameInput = string.Empty;
    private string _purchaseDateInput = DateTime.Now.ToString("MM/dd/yyyy");
    private string _purchaseStatusInput = string.Empty;
    private string _purchaseProductInput = string.Empty;
    private int _purchaseQuantityInput;
    private decimal _purchasePriceInput;
    private string _purchasePaymentModeInput = string.Empty;

    private string _returnVendorNameInput = string.Empty;
    private string _returnInvoiceInput = string.Empty;
    private string _returnDateInput = DateTime.Now.ToString("MM/dd/yyyy");
    private int _returnQuantityInput;
    private decimal _returnUnitPriceInput;

    private string _payBillVendorNameInput = string.Empty;
    private string _payBillInvoiceInput = string.Empty;
    private string _payBillDateInput = DateTime.Now.ToString("MM/dd/yyyy");
    private string _payBillModeInput = string.Empty;
    private decimal _payBillAmountInput;

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

    public string PurchasePaymentModeInput
    {
        get => _purchasePaymentModeInput;
        set => SetProperty(ref _purchasePaymentModeInput, value);
    }

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
        if (string.IsNullOrWhiteSpace(VendorNameInput) ||
            string.IsNullOrWhiteSpace(VendorCompanyInput) ||
            string.IsNullOrWhiteSpace(VendorEmailInput) ||
            string.IsNullOrWhiteSpace(VendorPhoneInput) ||
            string.IsNullOrWhiteSpace(VendorAddressInput) ||
            string.IsNullOrWhiteSpace(VendorAccountNameInput) ||
            string.IsNullOrWhiteSpace(VendorAccountNumberInput))
        {
            return;
        }

        _repository.AddVendor(
            new AddVendorInput
            {
                VendorName = VendorNameInput,
                CompanyName = VendorCompanyInput,
                EmailAddress = VendorEmailInput,
                PhoneNumber = VendorPhoneInput,
                Address = VendorAddressInput,
                AccountName = VendorAccountNameInput,
                AccountNumber = VendorAccountNumberInput
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
        if (string.IsNullOrWhiteSpace(PurchaseVendorNameInput) ||
            string.IsNullOrWhiteSpace(PurchaseDateInput) ||
            string.IsNullOrWhiteSpace(PurchaseStatusInput) ||
            string.IsNullOrWhiteSpace(PurchaseProductInput) ||
            PurchaseQuantityInput <= 0 ||
            PurchasePriceInput <= 0 ||
            string.IsNullOrWhiteSpace(PurchasePaymentModeInput))
        {
            return;
        }

        _repository.AddPurchaseInvoice(
            new AddPurchaseInvoiceInput
            {
                VendorName = PurchaseVendorNameInput,
                PurchaseDate = PurchaseDateInput,
                PurchaseStatus = PurchaseStatusInput,
                ProductName = PurchaseProductInput,
                Quantity = PurchaseQuantityInput,
                Price = PurchasePriceInput,
                PaymentMode = PurchasePaymentModeInput
            });

        IsPurchaseInvoiceOpen = false;
        PurchaseVendorNameInput = string.Empty;
        PurchaseStatusInput = string.Empty;
        PurchaseProductInput = string.Empty;
        PurchaseQuantityInput = 0;
        PurchasePriceInput = 0;
        PurchasePaymentModeInput = string.Empty;
        LoadData();
    }

    private void SavePurchaseReturn()
    {
        if (string.IsNullOrWhiteSpace(ReturnVendorNameInput) ||
            string.IsNullOrWhiteSpace(ReturnInvoiceInput) ||
            string.IsNullOrWhiteSpace(ReturnDateInput) ||
            ReturnQuantityInput <= 0 ||
            ReturnUnitPriceInput <= 0)
        {
            return;
        }

        _repository.AddPurchaseReturn(
            new AddPurchaseReturnInput
            {
                VendorName = ReturnVendorNameInput,
                InvoiceNumber = ReturnInvoiceInput,
                ReturnDate = ReturnDateInput,
                ReturnQuantity = ReturnQuantityInput,
                UnitPrice = ReturnUnitPriceInput
            });

        IsPurchaseReturnOpen = false;
        ReturnVendorNameInput = string.Empty;
        ReturnInvoiceInput = string.Empty;
        ReturnQuantityInput = 0;
        ReturnUnitPriceInput = 0;
        LoadData();
    }

    private void SavePayBill()
    {
        if (string.IsNullOrWhiteSpace(PayBillVendorNameInput) ||
            string.IsNullOrWhiteSpace(PayBillInvoiceInput) ||
            string.IsNullOrWhiteSpace(PayBillDateInput) ||
            string.IsNullOrWhiteSpace(PayBillModeInput) ||
            PayBillAmountInput <= 0)
        {
            return;
        }

        _repository.AddVendorPayment(
            new AddVendorPaymentInput
            {
                VendorName = PayBillVendorNameInput,
                InvoiceNumber = PayBillInvoiceInput,
                PaymentDate = PayBillDateInput,
                Amount = PayBillAmountInput,
                PaymentMode = PayBillModeInput
            });

        IsPayBillOpen = false;
        PayBillVendorNameInput = string.Empty;
        PayBillInvoiceInput = string.Empty;
        PayBillModeInput = string.Empty;
        PayBillAmountInput = 0;
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
