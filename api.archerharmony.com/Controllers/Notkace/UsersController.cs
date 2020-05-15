using api.archerharmony.com.DbContext;
using api.archerharmony.com.Models.Notkace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.archerharmony.com.Controllers.Notkace
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly NotkaceContext _context;

        public UsersController(NotkaceContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            List<User> users = await _context.User
                .AsNoTracking()
                .ToListAsync();

            if (users.Count == 0) return NoContent();

            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null) return NotFound();

            return user;
        }

        [HttpGet("owners")]
        public async Task<ActionResult<List<User>>> GetOwners()
        {
            var foo = await _context.User
                .Where(u => u.RoleId == 5)
                .OrderBy(u => u.FullName)
                .AsNoTracking()
                .ToListAsync();

            return foo;
        }
    }
}
