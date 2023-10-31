using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_153504_Klimkovich.API.Data;
using WEB_153504_Klimkovich.API.Services.ProductService;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectronicsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly string _imagesPath;
        private readonly string _appUri;

        public ElectronicsController(IWebHostEnvironment env, IConfiguration configuration, IProductService productService, ApplicationDbContext context)
        {
            _context = context;
            _productService = productService;
            _imagesPath = Path.Combine(env.WebRootPath, "Images");
            _appUri = configuration.GetSection("ImageUrl").Value;
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
        public async Task<ActionResult<ResponseData<Electronics>>> GetElectronics(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // PUT: api/Electronics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResponseData<Electronics>>> PutElectronics(int id, Electronics electronics)
        {
            var response = await _productService.UpdateProductAsync(id, electronics, null);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // POST: api/Electronics/5
        [Authorize]
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
        {
            var response = await _productService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // POST: api/Electronics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseData<Electronics>>> PostElectronics(Electronics electronics)
        {
            var response = await _productService.CreateProductAsync(electronics, null);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // DELETE: api/Electronics/5
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteElectronics(int id)
        {
            var response = await _productService.DeleteProductAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
