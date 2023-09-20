using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_153504_Klimkovich.API.Data;
using WEB_153504_Klimkovich.API.Services.ProductService;
using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectronicsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public ElectronicsController(IProductService productService, ApplicationDbContext context)
        {
            _context = context;
            _productService = productService;
        }

        // GET: api/Electronics/pageNo
        [HttpGet]
        [HttpGet("page{pageNo:int}")]
        [HttpGet("{category}")]
        [HttpGet("{category}/page{pageNo:int}")]
        public async Task<ActionResult<IEnumerable<Electronics>>> GetElectronics(string? category, int pageNo = 1, int pageSize = 3)
        {
            return Ok(await _productService.GetProductListAsync(category, pageNo, pageSize));
        }

        // GET: api/Electronics/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Electronics>> GetElectronics(int id)
        {
            if (_context.Electronics == null)
            {
                return NotFound();
            }
            var electronics = await _context.Electronics.FindAsync(id);

            if (electronics == null)
            {
                return NotFound();
            }

            return electronics;
        }

        // PUT: api/Electronics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutElectronics(int id, Electronics electronics)
        {
            if (id != electronics.Id)
            {
                return BadRequest();
            }

            _context.Entry(electronics).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectronicsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Electronics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Electronics>> PostElectronics(Electronics electronics)
        {
            if (_context.Electronics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Electronics'  is null.");
            }
            _context.Electronics.Add(electronics);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetElectronics", new { id = electronics.Id }, electronics);
        }

        // DELETE: api/Electronics/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteElectronics(int id)
        {
            if (_context.Electronics == null)
            {
                return NotFound();
            }
            var electronics = await _context.Electronics.FindAsync(id);
            if (electronics == null)
            {
                return NotFound();
            }

            _context.Electronics.Remove(electronics);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElectronicsExists(int id)
        {
            return (_context.Electronics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
