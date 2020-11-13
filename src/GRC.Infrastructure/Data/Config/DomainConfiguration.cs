using GRC.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GRC.Infrastructure.Data.Config
{
    public class DomainConfiguration : IEntityTypeConfiguration<Domain>
    {
        public void Configure(EntityTypeBuilder<Domain> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Domain.Controls));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
