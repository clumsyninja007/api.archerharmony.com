using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TelegramBot.Data;

// Used only by the EF Core tooling (migrations). Reads the "TelegramBot" connection string from
// user-secrets / environment so `dotnet ef database update` targets the real database; falls
// back to a placeholder so `migrations add` can scaffold offline (that path never connects).
public class TelegramBotContextFactory : IDesignTimeDbContextFactory<TelegramBotContext>
{
    public TelegramBotContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("TelegramBot")
            ?? "Server=localhost;Database=TelegramBot;Trusted_Connection=True;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<TelegramBotContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new TelegramBotContext(options);
    }
}
