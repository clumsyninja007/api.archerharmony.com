using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Notkace.Api.Data;

// Used only by the EF Core tooling (migrations). It reads the same "Notkace" connection string
// the app uses (user-secrets / environment) so `dotnet ef database update` targets the real
// database; when none is set it falls back to a placeholder so `migrations add` can still
// scaffold offline (that path never connects).
public class NotkaceContextFactory : IDesignTimeDbContextFactory<NotkaceContext>
{
    public NotkaceContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets(typeof(NotkaceContextFactory).Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("Notkace")
            ?? "Server=localhost;Database=Notkace;Trusted_Connection=True;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<NotkaceContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new NotkaceContext(options);
    }
}
