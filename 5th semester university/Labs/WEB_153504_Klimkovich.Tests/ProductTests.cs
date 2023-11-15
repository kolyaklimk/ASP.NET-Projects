using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Net.Http;
using WEB_153504_Klimkovich.Controllers;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;
using WEB_153504_Klimkovich.Services.CategoryService;
using WEB_153504_Klimkovich.Services.ProductService;

namespace WEB_153504_Klimkovich.Tests
{
    public class ProductTests
    {
        [Fact]
        public async Task Index_404_Categories()
        {
            var productServiceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(service => service.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>> { Success = false, ErrorMessage = "Categories error message" });
            var controller = new Product(productServiceMock.Object, categoryServiceMock.Object);

            var result = await controller.Index(null, 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Index_404_Product()
        {
            var productServiceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock.Setup(service => service.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>> { Success = true, Data = new List<Category>() });
            productServiceMock.Setup(service => service.GetProductListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Electronics>> { Success = false, ErrorMessage = "Products error message" });
            var controller = new Product(productServiceMock.Object, categoryServiceMock.Object);

            var result = await controller.Index(null, 1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Theory]
        [InlineData(null,null, "Всё")]
        [InlineData("category1", null, "Category 1")]
        [InlineData("category2", "XMLHttpRequest", "Category 2")]
        public async Task IndexCorrectData(string normalizedCategory, string xRequestedWith, string currentCategory)
        {
            var productServiceMock = new Mock<IProductService>();
            var categoryServiceMock = new Mock<ICategoryService>();
            var moqHttpContext = new Mock<HttpContext>();
            var header = new Dictionary<string, StringValues>() { ["x-requested-with"] = xRequestedWith };
            var categories = new List<Category> {
                new Category { NormalizedName = "category1", Name = "Category 1", Id = 1 },
                new Category { NormalizedName = "category2", Name = "Category 2", Id = 2 }
            };
            var products = new ListModel<Electronics>();
            products.Items = new(){
                new Electronics { Id = 1, Name = "Smartphone", Description = "Smartphone", CategoryId =  categories[0].Id, Category = categories[0], Price = 599.99m },
                new Electronics { Id = 3, Name = "Headphones", Description = "Headphones", CategoryId =  categories[1].Id, Category = categories[1], Price = 249.99m }
            };
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary(header));
            categoryServiceMock.Setup(service => service.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>> { Success = true, Data = categories });
            productServiceMock.Setup(service => service.GetProductListAsync(normalizedCategory, 1)).ReturnsAsync(new ResponseData<ListModel<Electronics>> { Success = true, Data = products });
            var controller = new Product(productServiceMock.Object, categoryServiceMock.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = moqHttpContext.Object, };

            var temp = await controller.Index(normalizedCategory, 1); ;// as ViewResult;
           
            Assert.NotNull(temp); 
            var result = Assert.IsType<ViewResult>(temp);
            Assert.Equal(products, result.Model);
            Assert.Equal(categories, result.ViewData["categories"]);
            Assert.Equal(currentCategory, result.ViewData["currentCategory"]);
        }
    }
}