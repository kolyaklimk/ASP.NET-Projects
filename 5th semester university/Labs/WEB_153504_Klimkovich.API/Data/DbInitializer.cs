using Microsoft.EntityFrameworkCore;
using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();

                var imageUrl = app.Configuration["ImageUrl"];

                var categories = new List<Category>
                {
                    new Category { Name="Смартфоны",
                        NormalizedName="smartphones"},
                    new Category { Name="Наушники",
                        NormalizedName="headphones"},
                    new Category { Name="Клавиатуры",
                        NormalizedName="keyboards"},
                    new Category {Name="USB",
                        NormalizedName="usb"},
                };

                var electronics = new List<Electronics>
                {
                    new Electronics { Name="Redmi Note 5",
                        Description="Snapdragon 636", Mime=".jpg",
                        Price = 199, Image=imageUrl+"/images/redmiNote5.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                    new Electronics { Name="Huawei P Smart 2019",
                        Description="Kirin 710", Mime=".jpg",
                        Price = 149, Image=imageUrl+"/images/huaweiPSmart.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                    new Electronics { Name="Huawei 2020",
                        Description="Kirin 810", Mime=".jpg",
                        Price = 230, Image=imageUrl+ "/images/huawei2020.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                    new Electronics { Name="Huawei 2017",
                        Description="Kirin 510", Mime=".jpg",
                        Price = 82, Image=imageUrl+"/images/huawei2017.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                    new Electronics { Name="Redmi Note 8 Pro",
                        Description="Snapdragon 712", Mime=".jpg",
                        Price = 222, Image=imageUrl+"/images/redmiNote8pro.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("smartphones"))},

                    new Electronics { Name="Beats",
                        Description="White, big", Mime=".jpg",
                        Price = 123, Image=imageUrl+"/images/beats.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("headphones"))},

                    new Electronics { Name="Sony xk840",
                        Description="Blue", Mime=".jpg",
                        Price = 235, Image=imageUrl+"/images/sonyxk840.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("headphones"))},

                    new Electronics { Name="AquaKey",
                        Description="Red", Mime=".jpg",
                        Price = 53, Image=imageUrl+"/images/aquakey.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("keyboards"))},

                    new Electronics { Name="RGB Serial 2820",
                        Description="RGB", Mime=".jpg",
                        Price = 75, Image=imageUrl+"/images/rgbserial2820.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("keyboards"))},

                    new Electronics { Name="GrassKey",
                        Description="Green", Mime=".jpg",
                        Price = 111, Image=imageUrl+"/images/grasskey.jpg",
                        Category= categories.Find(c=>c.NormalizedName.Equals("keyboards"))},
                };

                context.Categories.RemoveRange(context.Categories);
                context.Electronics.RemoveRange(context.Electronics);

                context.Categories.AddRange(categories);
                context.Electronics.AddRange(electronics);
                await context.SaveChangesAsync();
            }
        }
    }
}
