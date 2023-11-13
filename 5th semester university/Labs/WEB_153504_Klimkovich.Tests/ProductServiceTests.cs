using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WEB_153504_Klimkovich.API.Data;
using WEB_153504_Klimkovich.API.Services.ProductService;
using WEB_153504_Klimkovich.Domain.Entities;
using WEB_153504_Klimkovich.Domain.Models;

namespace WEB_153504_Klimkovich.Tests
{
    public class ProductServiceTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<ApplicationDbContext> _contextOptions;

        #region ConstructorAndDispose
        public ProductServiceTests()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            _contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = CreateContext())
            {
                context.Database.EnsureCreated();

                context.AddRange(
                    new Category { NormalizedName = "category1", Name = "Category 1", Id = 1 },
                    new Category { NormalizedName = "category2", Name = "Category 2", Id = 2 },
                    new Category { NormalizedName = "category3", Name = "Category 3", Id = 3 },
                    new Electronics { Id = 1, Name = "Smartphone1", Description = "Smartphone1", CategoryId = 1, Price = 199.99m },
                    new Electronics { Id = 2, Name = "Smartphone2", Description = "Smartphone2", CategoryId = 1, Price = 299.99m },
                    new Electronics { Id = 3, Name = "Smartphone3", Description = "Smartphone3", CategoryId = 1, Price = 399.99m },
                    new Electronics { Id = 4, Name = "Smartphone4", Description = "Smartphone4", CategoryId = 1, Price = 499.99m },
                    new Electronics { Id = 5, Name = "Smartphone5", Description = "Smartphone5", CategoryId = 1, Price = 599.99m },
                    new Electronics { Id = 6, Name = "Headphones1", Description = "Headphones1", CategoryId = 2, Price = 249.99m },
                    new Electronics { Id = 7, Name = "Headphones2", Description = "Headphones2", CategoryId = 2, Price = 149.99m },
                    new Electronics { Id = 8, Name = "Headphones31", Description = "Headphones31", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 9, Name = "Headphones32", Description = "Headphones32", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 10, Name = "Headphones33", Description = "Headphones33", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 11, Name = "Headphones34", Description = "Headphones34", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 12, Name = "Headphones35", Description = "Headphones35", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 13, Name = "Headphones36", Description = "Headphones36", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 14, Name = "Headphones37", Description = "Headphones37", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 15, Name = "Headphones38", Description = "Headphones38", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 16, Name = "Headphones39", Description = "Headphones39", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 17, Name = "Headphones13", Description = "Headphones13", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 18, Name = "Headphones23", Description = "Headphones23", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 19, Name = "Headphones36", Description = "Headphones36", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 21, Name = "Headphones43", Description = "Headphones43", CategoryId = 2, Price = 349.99m },
                    new Electronics { Id = 22, Name = "Laptop1", Description = "Laptop1", CategoryId = 3, Price = 449.99m }
                    );
                context.SaveChanges();
            }
        }

        ApplicationDbContext CreateContext() => new ApplicationDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public void ServiceReturnsFirstPageOfThreeItems(int pageNo)
        {
            using var context = CreateContext();
            var service = new ProductService(context, null, null);
            var result = service.GetProductListAsync(null, pageNo).Result;
            Assert.IsType<ResponseData<ListModel<Electronics>>>(result);
            Assert.True(result.Success);
            Assert.Equal(pageNo, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(7, result.Data.TotalPages);
            Assert.Equal(context.Electronics.FirstOrDefault(c => c.Id == (3 * pageNo - 2)), result.Data.Items[0]);
        }

        [Theory]
        [InlineData("category1", 2)]
        [InlineData("category3", 1)]
        public void ServiceReturnsCategoryItems(string? normalizedCategory, int totalPage)
        {
            using var context = CreateContext();
            var service = new ProductService(context, null, null);
            var result = service.GetProductListAsync(normalizedCategory).Result;
            Assert.IsType<ResponseData<ListModel<Electronics>>>(result);
            Assert.True(result.Success);
            Assert.Equal(totalPage, result.Data.TotalPages);
        }

        [Fact]
        public void ServiceReturnsMaxPageSize()
        {
            using var context = CreateContext();
            var service = new ProductService(context, null, null);
            var result = service.GetProductListAsync(null, pageSize:30).Result;
            Assert.IsType<ResponseData<ListModel<Electronics>>>(result);
            Assert.True(result.Success);
            Assert.Equal(20, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
        }

        [Fact]
        public void ServiceReturnsWrongPageNo()
        {
            using var context = CreateContext();
            var service = new ProductService(context, null, null);
            var result = service.GetProductListAsync(null, pageNo: 30).Result;
            Assert.IsType<ResponseData<ListModel<Electronics>>>(result);
            Assert.False(result.Success);
        }
    }
}
