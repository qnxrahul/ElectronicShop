using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ElectronicShop.Accounting.Desktop.Data;

public sealed class DatabaseInitializer
{
    private const string SchemaScriptFileName = "001_schema.sql";
    private const string SeedScriptFileName = "002_seed.sql";

    private readonly string _databasePath;
    private readonly string _scriptsDirectory;

    public DatabaseInitializer(string databasePath, string? scriptsDirectory = null)
    {
        _databasePath = databasePath;
        _scriptsDirectory = scriptsDirectory ?? Path.Combine(AppContext.BaseDirectory, "Data", "Scripts");
    }

    public void Initialize()
    {
        var folder = Path.GetDirectoryName(_databasePath);
        if (!string.IsNullOrWhiteSpace(folder))
        {
            Directory.CreateDirectory(folder);
        }

        using var connection = new SqliteConnection($"Data Source={_databasePath}");
        connection.Open();

        connection.Execute("PRAGMA foreign_keys = ON;");
        connection.Execute("PRAGMA journal_mode = WAL;");

        ExecuteScript(connection, SchemaScriptFileName);
        ApplyMigrations(connection);
        ExecuteScript(connection, SeedScriptFileName);
    }

    private void ExecuteScript(IDbConnection connection, string scriptFileName)
    {
        var scriptPath = Path.Combine(_scriptsDirectory, scriptFileName);
        if (!File.Exists(scriptPath))
        {
            throw new FileNotFoundException(
                $"Database script '{scriptFileName}' was not found at '{scriptPath}'. Ensure scripts are copied to output.");
        }

        var script = File.ReadAllText(scriptPath);
        connection.Execute(script);
    }

    private static void ApplyMigrations(IDbConnection connection)
    {
        EnsureColumn(connection, "BillingInvoices", "InvoiceFlowType", "TEXT NOT NULL DEFAULT 'Invoice'");
        EnsureColumn(connection, "InventoryItems", "ReorderLevel", "INTEGER NOT NULL DEFAULT 10");

        connection.Execute(
            """
            UPDATE BillingInvoices
            SET InvoiceFlowType = 'Invoice'
            WHERE InvoiceFlowType IS NULL OR InvoiceFlowType = '';
            """);

        connection.Execute(
            """
            UPDATE InventoryItems
            SET ReorderLevel = 10
            WHERE ReorderLevel IS NULL OR ReorderLevel <= 0;
            """);
    }

    private static void EnsureColumn(IDbConnection connection, string tableName, string columnName, string definition)
    {
        var columns = connection.Query<TableInfoColumn>($"PRAGMA table_info({tableName});")
            .Select(column => column.Name)
            .ToList();
        if (columns.Contains(columnName, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        connection.Execute($"ALTER TABLE {tableName} ADD COLUMN {columnName} {definition};");
    }

    private sealed class TableInfoColumn
    {
        public string Name { get; set; } = string.Empty;
    }
}
