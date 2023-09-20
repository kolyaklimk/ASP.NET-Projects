using Microsoft.EntityFrameworkCore;
using WEB_153504_Klimkovich.Domain.Entities;

namespace WEB_153504_Klimkovich.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Electronics> Electronics { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
