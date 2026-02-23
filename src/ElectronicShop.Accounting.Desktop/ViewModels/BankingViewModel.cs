using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class BankingViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;

    public BankingViewModel(AccountingRepository repository)
    {
        _repository = repository;
        LoadData();
    }

    public ObservableCollection<BankAccountCard> Accounts { get; } = [];

    public ObservableCollection<BankTransactionRow> Transactions { get; } = [];

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
}
