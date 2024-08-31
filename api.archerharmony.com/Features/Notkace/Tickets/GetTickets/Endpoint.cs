using api.archerharmony.com.Extensions;

namespace api.archerharmony.com.Features.Notkace.Tickets.GetTickets;

public class Endpoint(NotkaceContext context) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("");
        Group<TicketsGroup>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var query = context.HdTicket
            .Where(t => t.HdQueueId == 1)
            .Select(t => new GetTicket.Response
            {
                Ticket = t.Id,
                Title = t.Title,
                Priority = t.Priority.Name,
                Owner = t.Owner.FullName,
                Submitter = t.Submitter.FullName,
                Asset = t.Asset.Name,
                Status = t.Status.Name,
                ReferredTo = t.CustomFieldValue5,
                UserName = t.Owner.UserName,
                Dept = t.CustomFieldValue1,
                Location = t.CustomFieldValue2,
                PriOrd = t.Priority.Ordinal,
                StatOrd = t.Status.Ordinal,
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

        var orderedQuery = query.OrderBy(t => 0);

        if (!string.IsNullOrEmpty(req.Sort))
        {
            var sortFields = new SortFields(req.Sort);

            orderedQuery = sortFields.Fields
                .Aggregate(orderedQuery, (current, sortField) =>
                    sortField.Field switch
                    {
                        "ticket" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Ticket)
                            : current.ThenBy(t => t.Ticket),
                        "title" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Title)
                            : current.ThenBy(t => t.Title),
                        "priority" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.PriOrd)
                            : current.ThenBy(t => t.PriOrd),
                        "status" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.StatOrd)
                            : current.ThenBy(t => t.StatOrd),
                        "owner" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Owner)
                            : current.ThenBy(t => t.Owner),
                        "submitter" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Submitter)
                            : current.ThenBy(t => t.Submitter),
                        "asset" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Asset)
                            : current.ThenBy(t => t.Asset),
                        "referredto" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.ReferredTo)
                            : current.ThenBy(t => t.ReferredTo),
                        "dept" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Dept)
                            : current.ThenBy(t => t.Dept),
                        "location" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Location)
                            : current.ThenBy(t => t.Location),
                        "created" => sortField.SortDesc
                            ? current.ThenByDescending(t => t.Created)
                            : current.ThenBy(t => t.Created),
                        _ => current
                    }
                );
            
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
        
        var results = new Response
        {
            Result = await PaginatedList<GetTicket.Response>.CreateAsync(
                orderedQuery.AsNoTracking(), req.Page, req.PerPage)
        };

        if (results.Count == 0)
        {
            await SendNoContentAsync(ct);
        }

        Response = results;
    }
}