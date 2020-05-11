using UniqueWords.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniqueWords.Infrastructure.Persistence.Configs
{
    public class WordItemConfig : IEntityTypeConfiguration<WordItem>
    {
        public void Configure(EntityTypeBuilder<WordItem> builder)
        {
            builder.Property(wi => wi.Word)
                .HasMaxLength(ConfigDefaults.MaxWordLength)
                .IsRequired();

            builder
                .HasIndex(wi => wi.Word)
                .IsUnique();
        }
    }
}
