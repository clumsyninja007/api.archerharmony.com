using api.archerharmony.com.Classes.Notkace;
using api.archerharmony.com.DbContext;
using api.archerharmony.com.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.archerharmony.com.Controllers.Notkace
{
    [Route("[controller]")]
    [ApiController]
    public class HdTicketsController : ControllerBase
    {
        private readonly NotkaceContext _context;

        public HdTicketsController(NotkaceContext context)
        {
            _context = context;
        }

        // GET: api/HdTickets
        [HttpGet]
        public async Task<ActionResult<HdTicketsOutputDto>> GetHdTickets(
            [FromQuery] string assignee,
            string software,
            string referredTo,
            string department,
            string location,
            string excludedStatuses,
            string sort,
            int page = 1,
            int perPage = 20)
        {
            IQueryable<HdTicketsDto> query = _context.HdTicket
                .Where(t => t.HdQueueId == 1)
                .Select(t => new HdTicketsDto
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

            if (excludedStatuses != null)
            {
                string[] excludedList = excludedStatuses.Split(',');

                query = excludedList
                    .Aggregate(query, (current, status) => current.Where(t => t.Status != status));
            }

            if (assignee != null) query = query.Where(t => t.UserName == assignee);
            if (software != null) query = query.Where(t => t.Asset == software);
            if (referredTo != null) query = query.Where(t => t.ReferredTo == referredTo);
            if (department != null) query = query.Where(t => t.Dept == department);
            if (location != null) query = query.Where(t => t.Location == location);

            IOrderedQueryable<HdTicketsDto> orderedQuery = query.OrderBy(t => 0);

            var sortFields = new SortFields(sort);

            orderedQuery = sortFields.Fields?.Aggregate(orderedQuery, (current, sortField) =>
                               sortField.Field switch
                               {
                                   "ticket" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Ticket)
                                       : current.ThenBy(t => t.Ticket)),
                                   "title" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Title)
                                       : current.ThenBy(t => t.Title)),
                                   "priority" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.PriOrd)
                                       : current.ThenBy(t => t.PriOrd)),
                                   "status" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.StatOrd)
                                       : current.ThenBy(t => t.StatOrd)),
                                   "owner" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Owner)
                                       : current.ThenBy(t => t.Owner)),
                                   "submitter" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Submitter)
                                       : current.ThenBy(t => t.Submitter)),
                                   "asset" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Asset)
                                       : current.ThenBy(t => t.Asset)),
                                   "referredto" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.ReferredTo)
                                       : current.ThenBy(t => t.ReferredTo)),
                                   "dept" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Dept)
                                       : current.ThenBy(t => t.Dept)),
                                   "location" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Location)
                                       : current.ThenBy(t => t.Location)),
                                   "created" => (sortField.SortDesc
                                       ? current.ThenByDescending(t => t.Created)
                                       : current.ThenBy(t => t.Created)),
                                   _ => current
                               }
                           ) ?? orderedQuery;

            if (!sortFields.Contains("owner")) orderedQuery = orderedQuery.ThenBy(t => t.Owner);

            if (!sortFields.Contains("priority")) orderedQuery = orderedQuery.ThenBy(t => t.PriOrd);

            if (!sortFields.Contains("status")) orderedQuery = orderedQuery.ThenBy(t => t.StatOrd);

            var results = new HdTicketsOutputDto
            {
                Result = await PaginatedList<HdTicketsDto>.CreateAsync(
                    orderedQuery.AsNoTracking(), page, perPage)
            };

            if (results.Count == 0) return NoContent();

            return results;
        }

        // GET: api/HdTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HdTicketsDto>> GetHdTicket(long id)
        {
            var hdTicket = await _context.HdTicket
                .Select(t => new HdTicketsDto
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
                })
                .Where(t => t.Ticket == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (hdTicket == null) return NotFound();

            return hdTicket;
        }

        // GET: api/HdTickets/5/Info
        [HttpGet("{id}/Info")]
        public async Task<ActionResult<TicketInfoDto>> GetHdTicketInfo(long id)
        {
            var info = await _context.HdTicketChange
                .Select(c => new TicketInfoDto
                {
                    Ticket = c.HdTicket.Id,
                    Summary = c.HdTicket.Summary,
                    Comment = c.Comment,
                    Timestamp = c.Timestamp,
                    Commenter = c.User.FullName,
                    OwnersOnly = c.OwnersOnly
                })
                .Where(c => c.Ticket == id)
                .Where(c => c.Comment != null)
                .Where(c => c.Comment != "")
                .OrderByDescending(c => c.Timestamp)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (info == null) return NotFound();

            info.Comment = info.Comment.Replace("/packages/hd_attachments", "http://cty-k1k/packages/hd_attachments");

            const string pattern = @"<p>\s*</p>";

            info.Comment = Regex.Replace(info.Comment, pattern, "");

            return info;
        }
    }
}
