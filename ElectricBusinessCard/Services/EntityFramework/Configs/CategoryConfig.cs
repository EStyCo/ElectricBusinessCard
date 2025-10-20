using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectricBusinessCard.Services.EntityFramework.Configs
{
    public class CategoryConfig : IEntityTypeConfiguration<CategoryWork>
    {
        public void Configure(EntityTypeBuilder<CategoryWork> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.CategoryIndex).IsRequired();
            builder.Property(u => u.Name).IsRequired();
        }
    }
}
