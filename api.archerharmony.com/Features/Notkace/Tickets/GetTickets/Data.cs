using api.archerharmony.com.Entities.Context;
using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Notkace.Tickets.GetTickets;

public interface IData
{
    Task<Response> GetTickets(Request req, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public async Task<Response> GetTickets(Request req, CancellationToken ct = default)
    {
        var query = context.HdTickets
            .Where(t => t.HdQueueId == 1)
            .Select(t => new GetTicket.Response
            {
                Ticket = t.Id,
                Title = t.Title!,
                Priority = t.HdPriority!.Name,
                Owner = t.Owner!.FullName!,
                Submitter = t.Submitter!.FullName!,
                Asset = t.Asset!.Name,
                Status = t.HdStatus!.Name,
                ReferredTo = t.CustomFieldValue5!,
                UserName = t.Owner.UserName!,
                Dept = t.CustomFieldValue1!,
                Location = t.CustomFieldValue2!,
                PriOrd = t.HdPriority.Ordinal,
                StatOrd = t.HdStatus.Ordinal,
                Created = t.Created
            });

        if (!string.IsNullOrEmpty(req.ExcludedStatuses))
        {
            var excludedList = req.ExcludedStatuses.Split(',');

            query = excludedList
                .Aggregate(query, (current, status) => current.Where(t => t.Status != status));
        }

        if (!string.IsNullOrEmpty(req.Assignee))
        {
            query = query.Where(t => t.UserName == req.Assignee);
        }

        if (!string.IsNullOrEmpty(req.Software))
        {
            query = query.Where(t => t.Asset == req.Software);
        }

        if (!string.IsNullOrEmpty(req.ReferredTo))
        {
            query = query.Where(t => t.ReferredTo == req.ReferredTo);
        }

        if (!string.IsNullOrEmpty(req.Department))
        {
            query = query.Where(t => t.Dept == req.Department);
        }

        if (!string.IsNullOrEmpty(req.Location))
        {
            query = query.Where(t => t.Location == req.Location);
        }

        IOrderedQueryable<GetTicket.Response> orderedQuery;

        if (!string.IsNullOrEmpty(req.Sort))
        {
            var sortFields = new SortFields(req.Sort);

            if (sortFields.Fields.Count == 0)
            {
                orderedQuery = query.OrderBy(t => t.Owner)
                    .ThenBy(t => t.PriOrd)
                    .ThenBy(t => t.StatOrd);
            }
            else
            {
                var firstField = sortFields.Fields[0];
                orderedQuery = firstField.Field switch
                {
                    "ticket" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Ticket)
                        : query.OrderBy(t => t.Ticket),
                    "title" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Title)
                        : query.OrderBy(t => t.Title),
                    "priority" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.PriOrd)
                        : query.OrderBy(t => t.PriOrd),
                    "status" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.StatOrd)
                        : query.OrderBy(t => t.StatOrd),
                    "owner" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Owner)
                        : query.OrderBy(t => t.Owner),
                    "submitter" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Submitter)
                        : query.OrderBy(t => t.Submitter),
                    "asset" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Asset)
                        : query.OrderBy(t => t.Asset),
                    "referredto" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.ReferredTo)
                        : query.OrderBy(t => t.ReferredTo),
                    "dept" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Dept)
                        : query.OrderBy(t => t.Dept),
                    "location" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Location)
                        : query.OrderBy(t => t.Location),
                    "created" => firstField.SortDesc
                        ? query.OrderByDescending(t => t.Created)
                        : query.OrderBy(t => t.Created),
                    _ => query.OrderBy(t => 0)
                };

                for (var i = 1; i < sortFields.Fields.Count; i++)
                {
                    var sortField = sortFields.Fields[i];
                    orderedQuery = sortField.Field switch
                    {
                        "ticket" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Ticket)
                            : orderedQuery.ThenBy(t => t.Ticket),
                        "title" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Title)
                            : orderedQuery.ThenBy(t => t.Title),
                        "priority" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.PriOrd)
                            : orderedQuery.ThenBy(t => t.PriOrd),
                        "status" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.StatOrd)
                            : orderedQuery.ThenBy(t => t.StatOrd),
                        "owner" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Owner)
                            : orderedQuery.ThenBy(t => t.Owner),
                        "submitter" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Submitter)
                            : orderedQuery.ThenBy(t => t.Submitter),
                        "asset" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Asset)
                            : orderedQuery.ThenBy(t => t.Asset),
                        "referredto" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.ReferredTo)
                            : orderedQuery.ThenBy(t => t.ReferredTo),
                        "dept" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Dept)
                            : orderedQuery.ThenBy(t => t.Dept),
                        "location" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Location)
                            : orderedQuery.ThenBy(t => t.Location),
                        "created" => sortField.SortDesc
                            ? orderedQuery.ThenByDescending(t => t.Created)
                            : orderedQuery.ThenBy(t => t.Created),
                        _ => orderedQuery
                    };
                }

                if (!sortFields.Contains("owner"))
                {
                    orderedQuery = orderedQuery.ThenBy(t => t.Owner);
                }

                if (!sortFields.Contains("priority"))
                {
                    orderedQuery = orderedQuery.ThenBy(t => t.PriOrd);
                }

                if (!sortFields.Contains("status"))
                {
                    orderedQuery = orderedQuery.ThenBy(t => t.StatOrd);
                }
            }
        }
        else
        {
            orderedQuery = query.OrderBy(t => t.Owner)
                .ThenBy(t => t.PriOrd)
                .ThenBy(t => t.StatOrd);
        }
        
        var results = new Response
        {
            Result = await PaginatedList<GetTicket.Response>.CreateAsync(
                orderedQuery.AsNoTracking(), req.Page, req.PerPage, ct)
        };
        
        return results;
    }
}