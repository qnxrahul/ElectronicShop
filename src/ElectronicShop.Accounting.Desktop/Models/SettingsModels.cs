namespace ElectronicShop.Accounting.Desktop.Models;

public enum SettingsSection
{
    CompanyProfile,
    AccountsManagement,
    UserManagement,
    BackupSecurity
}

public sealed class CompanyProfileSettings
{
    public string CompanyName { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string LogoPath { get; set; } = string.Empty;
}

public sealed class LedgerAccountRow
{
    public string AccountName { get; set; } = string.Empty;

    public string AccountType { get; set; } = string.Empty;

    public decimal OpeningBalance { get; set; }

    public string CreatedDate { get; set; } = string.Empty;
}

public sealed class AccountSummaryCard
{
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public string AccentColor { get; set; } = "#D8E9FB";
}

public sealed class AppUserRow
{
    public string UserName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

public sealed class BackupLogRow
{
    public string Activity { get; set; } = string.Empty;

    public string DateAndTime { get; set; } = string.Empty;

    public string Size { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}

public sealed class BackupStatus
{
    public string CurrentStatus { get; set; } = string.Empty;

    public string LastSuccessfulBackup { get; set; } = string.Empty;

    public string NextScheduledBackup { get; set; } = string.Empty;

    public bool AutoBackupEnabled { get; set; }
}

public sealed class AddAccountInput
{
    public string AccountName { get; set; } = string.Empty;

    public string AccountType { get; set; } = string.Empty;

    public decimal OpeningBalance { get; set; }

    public string AsOfDate { get; set; } = string.Empty;
}

public sealed class AddUserInput
{
    public string UserName { get; set; } = string.Empty;

    public string MobileNumber { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
