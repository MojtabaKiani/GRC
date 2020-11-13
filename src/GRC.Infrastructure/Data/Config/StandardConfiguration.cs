using GRC.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GRC.Infrastructure.Data.Config
{
    public class StandardConfiguration : IEntityTypeConfiguration<Standard>
    {
        public void Configure(EntityTypeBuilder<Standard> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Standard.Domains));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
