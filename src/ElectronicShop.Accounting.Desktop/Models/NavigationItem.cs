namespace ElectronicShop.Accounting.Desktop.Models;

public enum NavigationTarget
{
    Dashboard,
    Billing,
    Inventory,
    Vendors,
    Banking,
    Expenses,
    Reports,
    Settings
}

public sealed class NavigationItem
{
    public NavigationItem(NavigationTarget target, string label, string glyph)
    {
        Target = target;
        Label = label;
        Glyph = glyph;
    }

    public NavigationTarget Target { get; }

    public string Label { get; }

    public string Glyph { get; }
}
