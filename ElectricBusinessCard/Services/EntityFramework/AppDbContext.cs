using ElectricBusinessCard.Services.EntityFramework.Configs;
using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectricBusinessCard.Services.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ElectroWork> ElectroWorks { get; set; }
        public DbSet<CategoryWork> CategoriesWorks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new WorkConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
