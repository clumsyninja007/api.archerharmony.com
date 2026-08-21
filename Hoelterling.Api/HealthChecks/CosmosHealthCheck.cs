using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Hoelterling.Api.HealthChecks;

// Readiness probe: confirms the process can actually reach Cosmos with whatever
// credential is in play (managed identity in Azure, account key locally). A plain
// liveness "return 200" wouldn't catch a broken identity/role assignment, which is
// the most likely failure mode after a deploy.
public sealed class CosmosHealthCheck(Container container) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await container.ReadContainerAsync(cancellationToken: cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Cosmos container unreachable", ex);
        }
    }
}
