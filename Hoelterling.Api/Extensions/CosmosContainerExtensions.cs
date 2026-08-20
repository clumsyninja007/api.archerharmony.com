using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.Extensions;

public static class CosmosContainerExtensions
{
    /// <summary>
    /// Runs a single-partition query and drains every page into a list.
    /// Centralizes the FeedIterator paging so use-case Data classes only supply the query and partition key.
    /// </summary>
    public static async Task<List<T>> QueryAsync<T>(
        this Container container,
        QueryDefinition query,
        PartitionKey partitionKey,
        CancellationToken ct = default)
    {
        using var iterator = container.GetItemQueryIterator<T>(
            query,
            requestOptions: new QueryRequestOptions { PartitionKey = partitionKey });

        var results = new List<T>();
        while (iterator.HasMoreResults)
        {
            results.AddRange(await iterator.ReadNextAsync(ct));
        }

        return results;
    }
}
