using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

// One-time snapshot copy of the slim Notkace dataset from the source KACE (MySQL/MariaDB)
// database into Azure SQL. Only the columns the read API needs are copied; table and column
// names are identical on both sides, so this is a straight column-subset transfer.
//
// Configure the two connection strings in appsettings.json (copy appsettings.example.json).
// Environment variables (ConnectionStrings__MySql / ConnectionStrings__SqlServer) override it.

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var mysqlConn = config.GetConnectionString("MySql");
var sqlConn = config.GetConnectionString("SqlServer");

if (string.IsNullOrWhiteSpace(mysqlConn) || string.IsNullOrWhiteSpace(sqlConn))
{
    Console.Error.WriteLine(
        "Missing connection strings. Copy appsettings.example.json to appsettings.json and fill in " +
        "ConnectionStrings:MySql and ConnectionStrings:SqlServer.");
    return 1;
}

// Column kinds drive both the DataTable CLR type and the value conversion.
Col Long(string name) => new(name, typeof(long));
Col Str(string name) => new(name, typeof(string));
Col Date(string name) => new(name, typeof(DateTime));
Col Bool(string name) => new(name, typeof(bool));

// Ordered parent -> child so FK targets exist before the rows that reference them.
var tables = new List<TableSpec>
{
    new("USER", [Long("ID"), Str("USER_NAME"), Str("FULL_NAME"), Long("ROLE_ID")]),
    new("ASSET", [Long("ID"), Long("ASSET_TYPE_ID"), Str("NAME")]),
    new("HD_PRIORITY", [Long("ID"), Str("NAME"), Long("ORDINAL")]),
    new("HD_STATUS", [Long("ID"), Str("NAME"), Long("ORDINAL")]),
    new("HD_TICKET",
    [
        Long("ID"), Str("TITLE"), Str("SUMMARY"), Long("HD_QUEUE_ID"), Date("CREATED"),
        Long("HD_PRIORITY_ID"), Long("HD_STATUS_ID"), Long("OWNER_ID"), Long("SUBMITTER_ID"),
        Long("ASSET_ID"), Str("CUSTOM_FIELD_VALUE1"), Str("CUSTOM_FIELD_VALUE2"),
        Str("CUSTOM_FIELD_VALUE5")
    ]),
    new("HD_TICKET_CHANGE",
    [
        Long("ID"), Long("HD_TICKET_ID"), Date("TIMESTAMP"), Long("USER_ID"),
        Str("COMMENT"), Bool("OWNERS_ONLY")
    ]),
};

await using var mysql = new MySqlConnection(mysqlConn);
await mysql.OpenAsync();
Console.WriteLine($"Connected to source MySQL: {mysql.Database}");

await using var sql = new SqlConnection(sqlConn);
await sql.OpenAsync();
Console.WriteLine($"Connected to target Azure SQL: {sql.Database}\n");

// Disable FK enforcement and clear existing rows (child -> parent) so the tool is re-runnable.
Console.WriteLine("Disabling FK constraints and clearing target tables...");
foreach (var t in Enumerable.Reverse(tables))
{
    await Exec(sql, $"ALTER TABLE [{t.Name}] NOCHECK CONSTRAINT ALL;");
    await Exec(sql, $"DELETE FROM [{t.Name}];");
}

// Load parent -> child.
Console.WriteLine("\nLoading snapshot...");
foreach (var t in tables)
{
    var table = new DataTable(t.Name);
    foreach (var c in t.Columns)
    {
        table.Columns.Add(new DataColumn(c.Name, c.ClrType) { AllowDBNull = true });
    }

    var columnList = string.Join(", ", t.Columns.Select(c => $"`{c.Name}`"));
    await using var cmd = new MySqlCommand($"SELECT {columnList} FROM `{t.Name}`;", mysql);
    await using var reader = await cmd.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        var row = table.NewRow();
        for (var i = 0; i < t.Columns.Count; i++)
        {
            row[i] = reader.IsDBNull(i) ? DBNull.Value : Convert(t.Columns[i], reader.GetValue(i));
        }
        table.Rows.Add(row);
    }

    using var bulk = new SqlBulkCopy(sql) { DestinationTableName = $"[{t.Name}]", BulkCopyTimeout = 600 };
    foreach (var c in t.Columns)
    {
        bulk.ColumnMappings.Add(c.Name, c.Name);
    }
    await bulk.WriteToServerAsync(table);
    Console.WriteLine($"  {t.Name,-18} {table.Rows.Count,7} rows");
}

// Re-enable + validate FKs. Orphan references (e.g. OWNER_ID pointing at a missing user)
// make the validation fail; we report it and leave that table's constraints untrusted rather
// than aborting the whole load.
Console.WriteLine("\nRe-validating FK constraints...");
foreach (var t in tables)
{
    try
    {
        await Exec(sql, $"ALTER TABLE [{t.Name}] WITH CHECK CHECK CONSTRAINT ALL;");
    }
    catch (SqlException ex)
    {
        Console.WriteLine($"  WARNING: {t.Name} has orphan FK references, left untrusted. ({ex.Message.Split('\n')[0]})");
    }
}

Console.WriteLine("\nDone.");
return 0;

static object Convert(Col col, object value) => col.ClrType switch
{
    var t when t == typeof(long) => System.Convert.ToInt64(value),
    var t when t == typeof(bool) => System.Convert.ToBoolean(value),
    var t when t == typeof(DateTime) => System.Convert.ToDateTime(value),
    _ => System.Convert.ToString(value) ?? string.Empty,
};

static async Task Exec(SqlConnection sql, string commandText)
{
    await using var cmd = new SqlCommand(commandText, sql) { CommandTimeout = 600 };
    await cmd.ExecuteNonQueryAsync();
}

internal readonly record struct Col(string Name, Type ClrType);

internal sealed record TableSpec(string Name, IReadOnlyList<Col> Columns);
