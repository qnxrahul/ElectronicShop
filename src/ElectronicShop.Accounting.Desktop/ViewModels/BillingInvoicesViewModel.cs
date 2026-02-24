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
    private bool _isCreateInvoiceDialogOpen;
    private bool _isInvoiceEditorOpen;
    private string _invoiceCustomerName = string.Empty;
    private string _invoicePhoneNumber = string.Empty;
    private string _invoicePaymentType = string.Empty;

    public BillingInvoicesViewModel(AccountingRepository repository)
    {
        _repository = repository;
        SelectInvoicesCommand = new RelayCommand(() => SelectedTab = "Invoices");
        SelectReturnsCommand = new RelayCommand(() => SelectedTab = "Return & Exchange");
        SelectCompletedCommand = new RelayCommand(() => SelectedTab = "Completed");
        OpenCreateInvoiceCommand = new RelayCommand(() => IsCreateInvoiceDialogOpen = true);
        CloseCreateInvoiceDialogCommand = new RelayCommand(() => IsCreateInvoiceDialogOpen = false);
        ResumeDraftCommand = new RelayCommand(OpenInvoiceEditor);
        StartFreshCommand = new RelayCommand(OpenInvoiceEditor);
        CloseInvoiceEditorCommand = new RelayCommand(() => IsInvoiceEditorOpen = false);
        SaveInvoiceCommand = new RelayCommand(() => SaveInvoice("Completed"));
        SaveDraftCommand = new RelayCommand(() => SaveInvoice("Draft"));

        EditorItems = [];

        LoadData();
    }

    public ObservableCollection<BillingInvoiceRow> Invoices { get; } = [];

    public ObservableCollection<InvoiceEditorLine> EditorItems { get; }

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
        }
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public bool IsCreateInvoiceDialogOpen
    {
        get => _isCreateInvoiceDialogOpen;
        set => SetProperty(ref _isCreateInvoiceDialogOpen, value);
    }

    public bool IsInvoiceEditorOpen
    {
        get => _isInvoiceEditorOpen;
        set => SetProperty(ref _isInvoiceEditorOpen, value);
    }

    public string InvoiceCustomerName
    {
        get => _invoiceCustomerName;
        set => SetProperty(ref _invoiceCustomerName, value);
    }

    public string InvoicePhoneNumber
    {
        get => _invoicePhoneNumber;
        set => SetProperty(ref _invoicePhoneNumber, value);
    }

    public string InvoicePaymentType
    {
        get => _invoicePaymentType;
        set => SetProperty(ref _invoicePaymentType, value);
    }

    public decimal InvoiceSubtotal => EditorItems.Sum(item => item.Amount);

    public decimal InvoiceGrandTotal => InvoiceSubtotal;

    public RelayCommand SelectInvoicesCommand { get; }

    public RelayCommand SelectReturnsCommand { get; }

    public RelayCommand SelectCompletedCommand { get; }

    public RelayCommand OpenCreateInvoiceCommand { get; }

    public RelayCommand CloseCreateInvoiceDialogCommand { get; }

    public RelayCommand ResumeDraftCommand { get; }

    public RelayCommand StartFreshCommand { get; }

    public RelayCommand CloseInvoiceEditorCommand { get; }

    public RelayCommand SaveInvoiceCommand { get; }

    public RelayCommand SaveDraftCommand { get; }

    private void LoadData()
    {
        var flowType = SelectedTab switch
        {
            "Return & Exchange" => "Return & Exchange",
            "Completed" => "Completed",
            _ => "Invoice"
        };

        Invoices.Clear();
        foreach (var invoice in _repository.GetBillingInvoices(flowType))
        {
            Invoices.Add(invoice);
        }
    }

    private void OpenInvoiceEditor()
    {
        IsCreateInvoiceDialogOpen = false;
        LoadEditorTemplate();
        IsInvoiceEditorOpen = true;
    }

    private void SaveInvoice(string status)
    {
        if (string.IsNullOrWhiteSpace(InvoiceCustomerName) ||
            string.IsNullOrWhiteSpace(InvoicePhoneNumber) ||
            string.IsNullOrWhiteSpace(InvoicePaymentType) ||
            EditorItems.Count == 0)
        {
            return;
        }

        _repository.AddBillingInvoice(
            new AddBillingInvoiceInput
            {
                CustomerName = InvoiceCustomerName,
                PhoneNumber = InvoicePhoneNumber,
                PaymentType = InvoicePaymentType,
                Status = status,
                InvoiceFlowType = SelectedTab == "Return & Exchange" ? "Return & Exchange" : SelectedTab == "Completed" ? "Completed" : "Invoice",
                TotalAmount = InvoiceGrandTotal,
                ItemCount = EditorItems.Sum(item => item.Quantity)
            });

        IsInvoiceEditorOpen = false;
        LoadData();
    }

    private void LoadEditorTemplate()
    {
        EditorItems.Clear();
        foreach (var line in _repository.GetInvoiceEditorTemplateLines())
        {
            EditorItems.Add(line);
        }

        OnPropertyChanged(nameof(InvoiceSubtotal));
        OnPropertyChanged(nameof(InvoiceGrandTotal));
    }
}
