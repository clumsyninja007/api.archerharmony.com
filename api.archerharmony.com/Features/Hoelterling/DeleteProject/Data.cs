using api.archerharmony.com.Entities.Entities;
using Dapper;

namespace api.archerharmony.com.Features.Hoelterling.DeleteProject;

public interface IData
{
    Task DeleteProject(int personId, int projectId);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(IDatabaseConnectionFactory connectionFactory) : IData
{
    public async Task DeleteProject(int personId, int projectId)
    {
        await using var conn = connectionFactory.CreateConnection(DatabaseType.Hoelterling);

        // Soft delete by setting is_active to false
        const string sql = """
            UPDATE project
            SET is_active = FALSE,
                updated_at = NOW()
            WHERE id = @ProjectId AND person_id = @PersonId
            """;

        await conn.ExecuteAsync(sql, new { ProjectId = projectId, PersonId = personId });
    }
}
