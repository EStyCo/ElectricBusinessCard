using ElectricBusinessCard.Services.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectricBusinessCard.Services.EntityFramework.Configs
{
    public class WorkConfig : IEntityTypeConfiguration<ElectroWork>
    {
        public void Configure(EntityTypeBuilder<ElectroWork> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(c => c.Category)
                .WithMany(c => c.Works)
                .HasForeignKey(c => c.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.WorkIndex).IsRequired();
            builder.Property(u => u.Name).IsRequired();
        }
    }
}
