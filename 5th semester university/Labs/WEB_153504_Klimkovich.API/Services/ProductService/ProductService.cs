using Microsoft.EntityFrameworkCore;
using WEB_153504_Klimkovich.API.Data;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.API.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly int _maxPageSize = 20;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ResponseData<ListModel<Electronics>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;

            var query = _context.Electronics
                .AsQueryable()
                .Where(d => categoryNormalizedName == null || d.Category.NormalizedName
                .Equals(categoryNormalizedName));

            int totalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);
            if (pageNo > totalPages)
                return new ResponseData<ListModel<Electronics>>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No such page"
                };

            var result = new ResponseData<ListModel<Electronics>>()
            {
                Data = new()
                {
                    Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync(),
                    CurrentPage = pageNo,
                    TotalPages = totalPages,
                }
            };
            return result;
        }

        public async Task<ResponseData<Electronics>> GetProductByIdAsync(int id)
        {
            if (_context.Electronics == null)
            {
                return new ResponseData<Electronics>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No items"
                };
            }
            var electronics = await _context.Electronics.FindAsync(id);

            if (electronics == null)
            {
                return new ResponseData<Electronics>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No items"
                };
            }

            return new ResponseData<Electronics>
            {
                Data = electronics,
                Success = true,
            };
        }

        public async Task<ResponseData<Electronics>> CreateProductAsync(Electronics electronics, IFormFile? formFile)
        {
            if (_context.Electronics == null)
            {
                return new ResponseData<Electronics>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No items"
                };
            }

            _context.Electronics.Add(electronics);
            await _context.SaveChangesAsync();

            return new ResponseData<Electronics>
            {
                Data = electronics,
                Success = true,
            };
        }

        public async Task<ResponseData<string>> DeleteProductAsync(int id)
        {
            if (_context.Electronics == null)
            {
                return new ResponseData<string>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No items"
                };
            }
            var electronics = await _context.Electronics.FindAsync(id);
            if (electronics == null)
            {
                return new ResponseData<string>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No find item"
                };
            }

            _context.Electronics.Remove(electronics);
            await _context.SaveChangesAsync();

            return new ResponseData<string>
            {
                Data = null,
                Success = true,
            };
        }

        public async Task<ResponseData<Electronics>> UpdateProductAsync(int id, Electronics electronics, IFormFile? formFile)
        {
            if (id != electronics.Id)
            {
                return new ResponseData<Electronics>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No items"
                };
            }

            var existingElectronics = await _context.Electronics.FindAsync(id);
            if (existingElectronics == null)
            {
                return new ResponseData<Electronics>
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "No items"
                };
            }

            _context.Entry(existingElectronics).CurrentValues.SetValues(electronics);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(_context.Electronics?.Any(e => e.Id == id)).GetValueOrDefault())
                {
                    return new ResponseData<Electronics>
                    {
                        Data = null,
                        Success = false,
                        ErrorMessage = "No items"
                    };
                }
                else
                {
                    throw;
                }
            }

            var responseData = new ResponseData<Electronics>
            {
                Data = electronics,
                Success = true
            };

            return responseData;
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>();
            var electronic = await _context.Electronics.FindAsync(id);
            if (electronic == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }
            var host = "https://" + _httpContextAccessor.HttpContext.Request.Host;
            var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            if (formFile != null)
            {
                // Удалить предыдущее изображение
                if (!String.IsNullOrEmpty(electronic.Image))
                {
                    var prevImage = Path.GetFileName(electronic.Image);
                    if (File.Exists(prevImage))
                    {
                        File.Delete(prevImage);
                    }
                }
                // Создать имя файла
                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);

                // Сохранить файл
                var imagePath = Path.Combine(imageFolder, fName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                // Указать имя файла в объекте
                electronic.Image = $"{host}/Images/{fName}";
                await _context.SaveChangesAsync();
            }
            responseData.Data = electronic.Image;
            return responseData;
        }
    }
}
