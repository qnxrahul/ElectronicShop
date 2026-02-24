using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class BankingViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private bool _isNewTransactionOpen;
    private string _transactionType = "Received";
    private string _transactionDate = DateTime.Now.ToString("MM/dd/yyyy");
    private string _selectedAccount = "HDFC Bank - Current";
    private decimal _transactionAmount;
    private string _transactionDescription = string.Empty;

    public BankingViewModel(AccountingRepository repository)
    {
        _repository = repository;
        OpenNewTransactionCommand = new RelayCommand(() => IsNewTransactionOpen = true);
        CloseNewTransactionCommand = new RelayCommand(() => IsNewTransactionOpen = false);
        SaveTransactionCommand = new RelayCommand(SaveTransaction);
        LoadData();
    }

    public ObservableCollection<BankAccountCard> Accounts { get; } = [];

    public ObservableCollection<BankTransactionRow> Transactions { get; } = [];

    public bool IsNewTransactionOpen
    {
        get => _isNewTransactionOpen;
        set => SetProperty(ref _isNewTransactionOpen, value);
    }

    public string TransactionType
    {
        get => _transactionType;
        set => SetProperty(ref _transactionType, value);
    }

    public string TransactionDate
    {
        get => _transactionDate;
        set => SetProperty(ref _transactionDate, value);
    }

    public string SelectedAccount
    {
        get => _selectedAccount;
        set => SetProperty(ref _selectedAccount, value);
    }

    public decimal TransactionAmount
    {
        get => _transactionAmount;
        set => SetProperty(ref _transactionAmount, value);
    }

    public string TransactionDescription
    {
        get => _transactionDescription;
        set => SetProperty(ref _transactionDescription, value);
    }

    public RelayCommand OpenNewTransactionCommand { get; }

    public RelayCommand CloseNewTransactionCommand { get; }

    public RelayCommand SaveTransactionCommand { get; }

    private void LoadData()
    {
        Accounts.Clear();
        foreach (var account in _repository.GetBankAccounts())
        {
            Accounts.Add(account);
        }

        Transactions.Clear();
        foreach (var transaction in _repository.GetBankTransactions())
        {
            Transactions.Add(transaction);
        }
    }

    private void SaveTransaction()
    {
        _repository.AddBankTransaction(
            new AddBankTransactionInput
            {
                TransactionType = string.IsNullOrWhiteSpace(TransactionType) ? "Received" : TransactionType,
                TransactionDate = string.IsNullOrWhiteSpace(TransactionDate) ? DateTime.Now.ToString("MM/dd/yyyy") : TransactionDate,
                AccountName = string.IsNullOrWhiteSpace(SelectedAccount) ? "Cash Account" : SelectedAccount,
                Amount = TransactionAmount <= 0 ? 100m : TransactionAmount,
                Description = string.IsNullOrWhiteSpace(TransactionDescription) ? "Manual transaction entry" : TransactionDescription
            });

        IsNewTransactionOpen = false;
        TransactionAmount = 0m;
        TransactionDescription = string.Empty;
        LoadData();
    }
}
