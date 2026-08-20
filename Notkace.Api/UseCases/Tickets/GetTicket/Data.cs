using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Notkace.Api.Data;

namespace Notkace.Api.UseCases.Tickets.GetTicket;

public interface IData
{
    Task<Response?> GetTicket(long id, CancellationToken ct = default);
}

[RegisterService<IData>(LifeTime.Scoped)]
public class Data(NotkaceContext context) : IData
{
    public Task<Response?> GetTicket(long id, CancellationToken ct = default)
    {
        return context.HdTickets
            .Where(t => t.Id == id)
            .Select(t => new Response
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
            })
            .FirstOrDefaultAsync(ct);
    }
}
