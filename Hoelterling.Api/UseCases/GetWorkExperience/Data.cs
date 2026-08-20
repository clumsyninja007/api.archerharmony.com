using FastEndpoints;
using Hoelterling.Api.Helpers;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.GetWorkExperience;

public interface IData
{
    public Task<List<WorkExperience>> GetWorkExperiences(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<List<WorkExperience>> GetWorkExperiences(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT c.id
                ,c.title
                ,c.company
                ,c.location
                ,c.startDate
                ,c.endDate
                ,c.skills
                ,c.localizations
            FROM c
            WHERE c.type = "workExperience"
                AND c.personId = @personId
            ORDER BY c.startDate DESC
            """;

        using var iterator = container.GetItemQueryIterator<WorkExperienceDoc>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey("workExperience") });

        var results = new List<WorkExperience>();
        while (iterator.HasMoreResults)
        {
            foreach (var doc in await iterator.ReadNextAsync(ct))
            {
                results.Add(new WorkExperience(
                    Id: doc.Id,
                    Title: LocalizationHelper.Localize(doc.Title, doc.Localizations, language, "title"),
                    Company: doc.Company, // NOT localized (proper noun)
                    Location: LocalizationHelper.Localize(doc.Location, doc.Localizations, language, "location"),
                    StartDate: doc.StartDate,
                    EndDate: doc.EndDate,
                    Skills: doc.Skills
                        .OrderBy(s => s.DisplayOrder)
                        .Select(s => LocalizationHelper.Localize(s.Skill, s.Localizations, language, "skill"))));
            }
        }
        return results;
    }
}
