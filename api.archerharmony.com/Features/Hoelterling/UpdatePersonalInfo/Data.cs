using api.archerharmony.com.Entities.Entities;
using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Hoelterling.UpdatePersonalInfo;

public interface IData
{
    Task UpdatePersonalInfo(int personId, PersonalInfoData en, PersonalInfoData de, string updatedBy, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task UpdatePersonalInfo(int personId, PersonalInfoData en, PersonalInfoData de, string updatedBy, CancellationToken ct = default)
    {
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);
        await conn.OpenAsync(ct);
        await using var transaction = await conn.BeginTransactionAsync(ct);

        try
        {
            // Update base table (using English values)
            const string updateBase = """
                UPDATE person
                SET name = @Name,
                    title = @Title,
                    hero_description = @HeroDescription,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                WHERE id = @PersonId
                """;

            await conn.ExecuteAsync(updateBase, new
            {
                PersonId = personId,
                Name = en.Name,
                Title = en.Title,
                HeroDescription = en.HeroDescription,
                UpdatedBy = updatedBy
            }, transaction, cancellationToken: ct);

            // Upsert localized data for English
            const string upsertEn = """
                INSERT INTO person_localized (person_id, language_code, title, hero_description, updated_by)
                VALUES (@PersonId, 'en', @Title, @HeroDescription, @UpdatedBy)
                ON DUPLICATE KEY UPDATE
                    title = @Title,
                    hero_description = @HeroDescription,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                """;

            await conn.ExecuteAsync(upsertEn, new
            {
                PersonId = personId,
                Title = en.Title,
                HeroDescription = en.HeroDescription,
                UpdatedBy = updatedBy
            }, transaction, cancellationToken: ct);

            // Upsert localized data for German
            const string upsertDe = """
                INSERT INTO person_localized (person_id, language_code, title, hero_description, updated_by)
                VALUES (@PersonId, 'de', @Title, @HeroDescription, @UpdatedBy)
                ON DUPLICATE KEY UPDATE
                    title = @Title,
                    hero_description = @HeroDescription,
                    updated_by = @UpdatedBy,
                    updated_at = NOW()
                """;

            await conn.ExecuteAsync(upsertDe, new
            {
                PersonId = personId,
                Title = de.Title,
                HeroDescription = de.HeroDescription,
                UpdatedBy = updatedBy
            }, transaction, cancellationToken: ct);

            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}