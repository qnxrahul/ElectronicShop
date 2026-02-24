using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class ExpensesViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private bool _isAddExpenseOpen;
    private string _expenseTitleInput = string.Empty;
    private decimal _expenseAmountInput;
    private string _expenseDateInput = DateTime.Now.ToString("MM/dd/yyyy");
    private string _expenseCategoryInput = "Utilities";
    private string _expensePaidFromInput = "Cash Account";

    public ExpensesViewModel(AccountingRepository repository)
    {
        _repository = repository;
        OpenAddExpenseCommand = new RelayCommand(() => IsAddExpenseOpen = true);
        CloseAddExpenseCommand = new RelayCommand(() => IsAddExpenseOpen = false);
        SaveExpenseCommand = new RelayCommand(SaveExpense);
        LoadData();
    }

    public ObservableCollection<ExpenseSummaryCard> SummaryCards { get; } = [];

    public ObservableCollection<ExpenseRow> Expenses { get; } = [];

    public ObservableCollection<ExpenseCategoryBreakdown> CategoryBreakdown { get; } = [];

    public bool IsAddExpenseOpen
    {
        get => _isAddExpenseOpen;
        set => SetProperty(ref _isAddExpenseOpen, value);
    }

    public string ExpenseTitleInput
    {
        get => _expenseTitleInput;
        set => SetProperty(ref _expenseTitleInput, value);
    }

    public decimal ExpenseAmountInput
    {
        get => _expenseAmountInput;
        set => SetProperty(ref _expenseAmountInput, value);
    }

    public string ExpenseDateInput
    {
        get => _expenseDateInput;
        set => SetProperty(ref _expenseDateInput, value);
    }

    public string ExpenseCategoryInput
    {
        get => _expenseCategoryInput;
        set => SetProperty(ref _expenseCategoryInput, value);
    }

    public string ExpensePaidFromInput
    {
        get => _expensePaidFromInput;
        set => SetProperty(ref _expensePaidFromInput, value);
    }

    public RelayCommand OpenAddExpenseCommand { get; }

    public RelayCommand CloseAddExpenseCommand { get; }

    public RelayCommand SaveExpenseCommand { get; }

    private void LoadData()
    {
        ReplaceCollection(SummaryCards, _repository.GetExpenseSummaryCards());
        ReplaceCollection(Expenses, _repository.GetExpenses());

        var breakdown = _repository.GetExpenseCategoryBreakdown().ToList();
        foreach (var row in breakdown)
        {
            row.BarWidth = 240 * (row.Percentage / 100d);
        }

        ReplaceCollection(CategoryBreakdown, breakdown);
    }

    private void SaveExpense()
    {
        _repository.AddExpense(
            new AddExpenseInput
            {
                Title = string.IsNullOrWhiteSpace(ExpenseTitleInput) ? "General expense" : ExpenseTitleInput,
                Amount = ExpenseAmountInput <= 0 ? 100m : ExpenseAmountInput,
                ExpenseDate = string.IsNullOrWhiteSpace(ExpenseDateInput) ? DateTime.Now.ToString("MM/dd/yyyy") : ExpenseDateInput,
                Category = string.IsNullOrWhiteSpace(ExpenseCategoryInput) ? "Other" : ExpenseCategoryInput,
                PaidFrom = string.IsNullOrWhiteSpace(ExpensePaidFromInput) ? "Cash Account" : ExpensePaidFromInput
            });

        IsAddExpenseOpen = false;
        ExpenseTitleInput = string.Empty;
        ExpenseAmountInput = 0m;
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
