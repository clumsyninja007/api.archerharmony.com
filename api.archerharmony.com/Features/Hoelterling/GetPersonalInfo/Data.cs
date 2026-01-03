using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetPersonalInfo;

public interface IData
{
    Task<PersonWithDescription?> GetPersonalInfo(int id, string language, CancellationToken ct = default);
    Task<List<ContactInfo>> GetContactInfo(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task<PersonWithDescription?> GetPersonalInfo(int id, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT
                p.name,
                COALESCE(pl.title, p.title) AS title,
                COALESCE(pl.hero_description, p.hero_description) AS heroDescription
            FROM person p
            LEFT JOIN person_localized pl
                ON p.id = pl.person_id
                AND pl.language_code = @language
            WHERE p.id = @id
            """;
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        var command = new CommandDefinition(query, new { id, language }, cancellationToken: ct);
        return await conn.QueryFirstOrDefaultAsync<PersonWithDescription>(command);
    }

    public async Task<List<ContactInfo>> GetContactInfo(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT
                COALESCE(cl.label, c.label) AS label,
                c.link,
                c.icon
            FROM contact c
            LEFT JOIN contact_localized cl
                ON c.id = cl.contact_id
                AND cl.language_code = @language
            WHERE c.person_id = @personId
            """;
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        var command = new CommandDefinition(query, new { personId, language }, cancellationToken: ct);
        var contact = await conn.QueryAsync<ContactInfo>(command);
        return contact.ToList();
    }
}

public record PersonWithDescription(string Name, string Title, string? HeroDescription);