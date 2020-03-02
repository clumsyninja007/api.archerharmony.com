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
    public class AssetsController : ControllerBase
    {
        private readonly NotkaceContext _context;

        public AssetsController(NotkaceContext context)
        {
            _context = context;
        }

        // GET: api/Assets
        [HttpGet]
        public async Task<ActionResult<List<Asset>>> GetAsset()
        {
            List<Asset> assets = await _context.Asset
                .AsNoTracking()
                .ToListAsync();

            if (assets.Count == 0) return NoContent();

            return assets;
        }

        // GET: api/Assets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asset>> GetAsset(long id)
        {
            var asset = await _context.Asset.FindAsync(id);

            if (asset == null) return NotFound();

            return asset;
        }

        // GET: api/Assets/Type/5
        [HttpGet("Type/{id}")]
        public async Task<ActionResult<List<Asset>>> GetAssetsByType(long id)
        {
            List<Asset> assets = await _context.Asset
                .Where(a => a.AssetTypeId == id)
                .OrderBy(a => a.Name)
                .AsNoTracking()
                .ToListAsync();

            if (assets.Count == 0) return NoContent();

            return assets;
        }
    }
}
