using ElectronicShop.Accounting.Desktop.Infrastructure;

namespace ElectronicShop.Accounting.Desktop.ViewModels;

public sealed class PlaceholderViewModel : ViewModelBase
{
    private string _title = "Coming Soon";
    private string _description = "This module will be implemented in the next iteration.";

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
}
