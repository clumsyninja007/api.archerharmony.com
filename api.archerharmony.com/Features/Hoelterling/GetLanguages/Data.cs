using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.GetLanguages;

public interface IData
{
    public Task<List<string>> GetLanguages(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory databaseConnectionFactory) : IData
{
    public async Task<List<string>> GetLanguages(int personId, string language, CancellationToken ct = default)
    {
        await using var conn = databaseConnectionFactory.CreateConnection(DatabaseType.Hoelterling);

        const string query =
            """
            SELECT CONCAT(
                COALESCE(pll.language, pl.language),
                ' (',
                COALESCE(pll.proficiency_level, pl.proficiency_level),
                ')'
            ) AS language_display
            FROM person_languages pl
            LEFT JOIN person_languages_localized pll
                ON pl.id = pll.person_language_id
                AND pll.language_code = @language
            WHERE pl.person_id = @personId
                AND pl.is_active = TRUE
            ORDER BY pl.display_order
            """;

        var command = new CommandDefinition(query, new { personId, language }, cancellationToken: ct);
        var languages = await conn.QueryAsync<string>(command);
        return languages.ToList();
    }
}