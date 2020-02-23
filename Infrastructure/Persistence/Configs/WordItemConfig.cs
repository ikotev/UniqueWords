namespace UniqueWords.Infrastructure.Persistence.Configs
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
