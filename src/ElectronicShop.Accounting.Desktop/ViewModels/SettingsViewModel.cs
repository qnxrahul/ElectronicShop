using System.Collections.ObjectModel;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.Infrastructure;
using ElectronicShop.Accounting.Desktop.Models;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class SettingsViewModel : ViewModelBase
{
    private readonly AccountingRepository _repository;
    private bool _isLoading;
    private SettingsSection _selectedSection = SettingsSection.CompanyProfile;
    private string _companyName = string.Empty;
    private string _mobileNumber = string.Empty;
    private string _emailAddress = string.Empty;
    private string _address = string.Empty;
    private bool _isAddAccountOpen;
    private bool _isAddUserOpen;
    private bool _autoBackupEnabled;

    private string _newAccountName = string.Empty;
    private string _newAccountType = "Bank";
    private decimal _newAccountOpeningBalance;
    private string _newAccountAsOfDate = DateTime.Now.ToString("MM/dd/yyyy");

    private string _newUserName = string.Empty;
    private string _newUserMobileNumber = string.Empty;
    private string _newUserPassword = string.Empty;
    private string _newUserRole = "Accountant";

    public SettingsViewModel(AccountingRepository repository)
    {
        _repository = repository;

        SelectCompanyProfileCommand = new RelayCommand(() => SelectedSection = SettingsSection.CompanyProfile);
        SelectAccountsManagementCommand = new RelayCommand(() => SelectedSection = SettingsSection.AccountsManagement);
        SelectUserManagementCommand = new RelayCommand(() => SelectedSection = SettingsSection.UserManagement);
        SelectBackupSecurityCommand = new RelayCommand(() => SelectedSection = SettingsSection.BackupSecurity);

        SaveCompanyProfileCommand = new RelayCommand(SaveCompanyProfile);
        OpenAddAccountCommand = new RelayCommand(() => IsAddAccountOpen = true);
        CloseAddAccountCommand = new RelayCommand(() => IsAddAccountOpen = false);
        SaveAddAccountCommand = new RelayCommand(SaveAddAccount);

        OpenAddUserCommand = new RelayCommand(() => IsAddUserOpen = true);
        CloseAddUserCommand = new RelayCommand(() => IsAddUserOpen = false);
        SaveAddUserCommand = new RelayCommand(SaveAddUser);

        RunBackupNowCommand = new RelayCommand(RunBackupNow);
        RunRestoreCommand = new RelayCommand(RunRestore);

        LoadData();
    }

    public ObservableCollection<AccountSummaryCard> AccountSummaryCards { get; } = [];

    public ObservableCollection<LedgerAccountRow> LedgerAccounts { get; } = [];

    public ObservableCollection<AppUserRow> AppUsers { get; } = [];

    public ObservableCollection<BackupLogRow> BackupLogs { get; } = [];

    public SettingsSection SelectedSection
    {
        get => _selectedSection;
        set
        {
            if (!SetProperty(ref _selectedSection, value))
            {
                return;
            }

            OnPropertyChanged(nameof(IsCompanyProfileVisible));
            OnPropertyChanged(nameof(IsAccountsVisible));
            OnPropertyChanged(nameof(IsUsersVisible));
            OnPropertyChanged(nameof(IsBackupVisible));
        }
    }

    public bool IsCompanyProfileVisible => SelectedSection == SettingsSection.CompanyProfile;

    public bool IsAccountsVisible => SelectedSection == SettingsSection.AccountsManagement;

    public bool IsUsersVisible => SelectedSection == SettingsSection.UserManagement;

    public bool IsBackupVisible => SelectedSection == SettingsSection.BackupSecurity;

    public string CompanyName
    {
        get => _companyName;
        set => SetProperty(ref _companyName, value);
    }

    public string MobileNumber
    {
        get => _mobileNumber;
        set => SetProperty(ref _mobileNumber, value);
    }

    public string EmailAddress
    {
        get => _emailAddress;
        set => SetProperty(ref _emailAddress, value);
    }

    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public bool IsAddAccountOpen
    {
        get => _isAddAccountOpen;
        set => SetProperty(ref _isAddAccountOpen, value);
    }

    public bool IsAddUserOpen
    {
        get => _isAddUserOpen;
        set => SetProperty(ref _isAddUserOpen, value);
    }

    public bool AutoBackupEnabled
    {
        get => _autoBackupEnabled;
        set
        {
            if (!SetProperty(ref _autoBackupEnabled, value) || _isLoading)
            {
                return;
            }

            _repository.SetAutoBackupEnabled(value);
            _repository.AddBackupLog(value ? "Auto backup enabled" : "Auto backup disabled", "Success");
            LoadBackupDataOnly();
        }
    }

    public string NewAccountName
    {
        get => _newAccountName;
        set => SetProperty(ref _newAccountName, value);
    }

    public string NewAccountType
    {
        get => _newAccountType;
        set => SetProperty(ref _newAccountType, value);
    }

    public decimal NewAccountOpeningBalance
    {
        get => _newAccountOpeningBalance;
        set => SetProperty(ref _newAccountOpeningBalance, value);
    }

    public string NewAccountAsOfDate
    {
        get => _newAccountAsOfDate;
        set => SetProperty(ref _newAccountAsOfDate, value);
    }

    public string NewUserName
    {
        get => _newUserName;
        set => SetProperty(ref _newUserName, value);
    }

    public string NewUserMobileNumber
    {
        get => _newUserMobileNumber;
        set => SetProperty(ref _newUserMobileNumber, value);
    }

    public string NewUserPassword
    {
        get => _newUserPassword;
        set => SetProperty(ref _newUserPassword, value);
    }

    public string NewUserRole
    {
        get => _newUserRole;
        set => SetProperty(ref _newUserRole, value);
    }

    public string BackupCurrentStatus { get; private set; } = string.Empty;

    public string LastSuccessfulBackup { get; private set; } = string.Empty;

    public string NextScheduledBackup { get; private set; } = string.Empty;

    public RelayCommand SelectCompanyProfileCommand { get; }

    public RelayCommand SelectAccountsManagementCommand { get; }

    public RelayCommand SelectUserManagementCommand { get; }

    public RelayCommand SelectBackupSecurityCommand { get; }

    public RelayCommand SaveCompanyProfileCommand { get; }

    public RelayCommand OpenAddAccountCommand { get; }

    public RelayCommand CloseAddAccountCommand { get; }

    public RelayCommand SaveAddAccountCommand { get; }

    public RelayCommand OpenAddUserCommand { get; }

    public RelayCommand CloseAddUserCommand { get; }

    public RelayCommand SaveAddUserCommand { get; }

    public RelayCommand RunBackupNowCommand { get; }

    public RelayCommand RunRestoreCommand { get; }

    private void LoadData()
    {
        _isLoading = true;

        var companyProfile = _repository.GetCompanyProfile();
        CompanyName = companyProfile.CompanyName;
        MobileNumber = companyProfile.MobileNumber;
        EmailAddress = companyProfile.EmailAddress;
        Address = companyProfile.Address;

        ReplaceCollection(AccountSummaryCards, _repository.GetAccountSummaryCards());
        ReplaceCollection(LedgerAccounts, _repository.GetLedgerAccounts());
        ReplaceCollection(AppUsers, _repository.GetAppUsers());

        var backupStatus = _repository.GetBackupStatus();
        BackupCurrentStatus = backupStatus.CurrentStatus;
        LastSuccessfulBackup = backupStatus.LastSuccessfulBackup;
        NextScheduledBackup = backupStatus.NextScheduledBackup;
        AutoBackupEnabled = backupStatus.AutoBackupEnabled;
        OnPropertyChanged(nameof(BackupCurrentStatus));
        OnPropertyChanged(nameof(LastSuccessfulBackup));
        OnPropertyChanged(nameof(NextScheduledBackup));

        ReplaceCollection(BackupLogs, _repository.GetBackupLogs());

        _isLoading = false;
    }

    private void LoadBackupDataOnly()
    {
        _isLoading = true;
        var backupStatus = _repository.GetBackupStatus();
        BackupCurrentStatus = backupStatus.CurrentStatus;
        LastSuccessfulBackup = backupStatus.LastSuccessfulBackup;
        NextScheduledBackup = backupStatus.NextScheduledBackup;
        AutoBackupEnabled = backupStatus.AutoBackupEnabled;
        OnPropertyChanged(nameof(BackupCurrentStatus));
        OnPropertyChanged(nameof(LastSuccessfulBackup));
        OnPropertyChanged(nameof(NextScheduledBackup));
        ReplaceCollection(BackupLogs, _repository.GetBackupLogs());
        _isLoading = false;
    }

    private void SaveCompanyProfile()
    {
        _repository.SaveCompanyProfile(
            new CompanyProfileSettings
            {
                CompanyName = string.IsNullOrWhiteSpace(CompanyName) ? "Company Name" : CompanyName,
                MobileNumber = string.IsNullOrWhiteSpace(MobileNumber) ? "NA" : MobileNumber,
                EmailAddress = string.IsNullOrWhiteSpace(EmailAddress) ? "info@company.com" : EmailAddress,
                Address = string.IsNullOrWhiteSpace(Address) ? "Address not provided" : Address,
                LogoPath = string.Empty
            });
    }

    private void SaveAddAccount()
    {
        _repository.AddLedgerAccount(
            new AddAccountInput
            {
                AccountName = string.IsNullOrWhiteSpace(NewAccountName) ? "New Account" : NewAccountName,
                AccountType = string.IsNullOrWhiteSpace(NewAccountType) ? "Bank" : NewAccountType,
                OpeningBalance = NewAccountOpeningBalance,
                AsOfDate = string.IsNullOrWhiteSpace(NewAccountAsOfDate) ? DateTime.Now.ToString("MM/dd/yyyy") : NewAccountAsOfDate
            });

        IsAddAccountOpen = false;
        NewAccountName = string.Empty;
        NewAccountType = "Bank";
        NewAccountOpeningBalance = 0m;
        NewAccountAsOfDate = DateTime.Now.ToString("MM/dd/yyyy");
        ReplaceCollection(AccountSummaryCards, _repository.GetAccountSummaryCards());
        ReplaceCollection(LedgerAccounts, _repository.GetLedgerAccounts());
    }

    private void SaveAddUser()
    {
        _repository.AddAppUser(
            new AddUserInput
            {
                UserName = string.IsNullOrWhiteSpace(NewUserName) ? "New User" : NewUserName,
                MobileNumber = string.IsNullOrWhiteSpace(NewUserMobileNumber) ? "+1 (000) 000-0000" : NewUserMobileNumber,
                Password = string.IsNullOrWhiteSpace(NewUserPassword) ? "password123" : NewUserPassword,
                Role = string.IsNullOrWhiteSpace(NewUserRole) ? "Accountant" : NewUserRole
            });

        IsAddUserOpen = false;
        NewUserName = string.Empty;
        NewUserMobileNumber = string.Empty;
        NewUserPassword = string.Empty;
        NewUserRole = "Accountant";
        ReplaceCollection(AppUsers, _repository.GetAppUsers());
    }

    private void RunBackupNow()
    {
        _repository.AddBackupLog("Manual Backup (Admin)", "Success");
        LoadBackupDataOnly();
    }

    private void RunRestore()
    {
        _repository.AddBackupLog("Restore Data", "Success");
        LoadBackupDataOnly();
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
