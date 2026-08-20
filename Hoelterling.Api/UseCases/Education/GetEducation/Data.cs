using FastEndpoints;
using Hoelterling.Api.Extensions;
using Hoelterling.Api.Helpers;
using Hoelterling.Api.UseCases.Education.Dtos;
using Microsoft.Azure.Cosmos;

namespace Hoelterling.Api.UseCases.Education.GetEducation;

public interface IData
{
    Task<List<EducationRecord>> GetEducation(int personId, string language, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(Container container) : IData
{
    public async Task<List<EducationRecord>> GetEducation(int personId, string language, CancellationToken ct = default)
    {
        const string query =
            """
            SELECT c.school
                ,c.degreeEarned
                ,c.major
                ,c.startDate
                ,c.endDate
                ,c.localizations
            FROM c
            WHERE c.type = "education"
                AND c.personId = @personId
            """;

        var docs = await container.QueryAsync<EducationDocument>(
            new QueryDefinition(query).WithParameter("@personId", personId),
            new PartitionKey(EducationDocument.PartitionKeyValue), ct);

        return docs.Select(doc => new EducationRecord
        {
            School = LocalizationHelper.Localize(doc.School, doc.Localizations, language, "school"),
            // degreeEarned & major are nullable: only localize when there's a base value to fall back to
            Degree = doc.DegreeEarned is null
                ? null
                : LocalizationHelper.Localize(doc.DegreeEarned, doc.Localizations, language, "degreeEarned"),
            Major = doc.Major is null
                ? null
                : LocalizationHelper.Localize(doc.Major, doc.Localizations, language, "major"),
            StartDate = doc.StartDate,
            EndDate = doc.EndDate
        }).ToList();
    }
}
