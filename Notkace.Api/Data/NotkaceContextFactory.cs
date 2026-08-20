using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notkace.Api.Data;

// Used only by the EF Core tooling (migrations). At runtime the context is configured in
// Program.cs from the "Notkace" connection string; migration scaffolding never connects, so
// a placeholder is enough here.
public class NotkaceContextFactory : IDesignTimeDbContextFactory<NotkaceContext>
{
    public NotkaceContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<NotkaceContext>()
            .UseSqlServer("Server=localhost;Database=Notkace;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new NotkaceContext(options);
    }
}
