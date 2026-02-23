using System.IO;
using System.Windows;
using ElectronicShop.Accounting.Desktop.Data;
using ElectronicShop.Accounting.Desktop.ViewModels;

namespace ElectronicShop.Accounting.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var appDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ElectronicShop",
            "Accounting");
        var databasePath = Path.Combine(appDataFolder, "accounting.db");

        var initializer = new DatabaseInitializer(databasePath);
        initializer.Initialize();

        var repository = new AccountingRepository(databasePath);
        var mainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel(repository)
        };

        MainWindow = mainWindow;
        mainWindow.Show();
    }
}
