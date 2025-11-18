using api.archerharmony.com.Entities.Entities;
using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public interface IData
{
    Task<Person?> GetPersonalInfo(int id, CancellationToken ct = default);
    Task<List<ContactInfo>> GetContactInfo(int personId, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task<Person?> GetPersonalInfo(int id, CancellationToken ct = default)
    {
        const string query = "SELECT name, title FROM person WHERE id = @id";
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        return await conn.QueryFirstOrDefaultAsync<Person>(query, new { id }, ct);
    }

    public async Task<List<ContactInfo>> GetContactInfo(int personId, CancellationToken ct = default)
    {
        const string query = "SELECT label, link, icon FROM contact WHERE person_id = @personId";
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        var contact = await conn.QueryAsync<ContactInfo>(query, new { personId }, ct);
        return contact.ToList();
    }
}