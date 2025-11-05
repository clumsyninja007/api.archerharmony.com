using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public interface IData
{
    Task<Person?> GetPersonalInfo(int id);
    Task<List<ContactInfo>> GetContactInfo(int personId);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task<Person?> GetPersonalInfo(int id)
    {
        const string query = "SELECT name, title FROM person WHERE id = @id";
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        return await conn.QueryFirstOrDefaultAsync<Person>(query, new { id });
    }

    public async Task<List<ContactInfo>> GetContactInfo(int personId)
    {
        const string query = "SELECT label, link, icon FROM contact WHERE person_id = @personId";
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        var contact = await conn.QueryAsync<ContactInfo>(query, new { personId });
        return contact.ToList();
    }
}