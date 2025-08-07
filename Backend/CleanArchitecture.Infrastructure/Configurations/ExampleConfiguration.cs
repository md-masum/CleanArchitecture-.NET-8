using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations
{
    public class ExampleConfiguration : IEntityTypeConfiguration<Example>
    {
        public void Configure(EntityTypeBuilder<Example> builder)
        {
            builder.Property(t => t.Title1).HasMaxLength(50).IsRequired();
            builder.Property(t => t.Title2).HasMaxLength(50).IsRequired();
            builder.Property(t => t.Title3).IsRequired();
        }
    }
}