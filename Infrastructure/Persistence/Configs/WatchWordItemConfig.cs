namespace UniqueWords.Infrastructure.Persistence.Configs
{
    using System.Collections.Generic;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class WatchWordItemConfig : IEntityTypeConfiguration<WatchWordItem>
    {
        public void Configure(EntityTypeBuilder<WatchWordItem> builder)
        {
            builder.Property(wwi => wwi.Word)
                .HasMaxLength(ConfigDefaults.MaxWordLength)
                .IsRequired();

            builder
                .HasIndex(wwi => wwi.Word)
                .IsUnique();

            Seed(builder);
        }

        private void Seed(EntityTypeBuilder<WatchWordItem> builder)
        {
            var data = new List<WatchWordItem>
            {
                new WatchWordItem{Id = 1, Word = "horse"},
                new WatchWordItem{Id = 2, Word = "zebra"},
                new WatchWordItem{Id = 3, Word = "dog"},
            };

            builder.HasData(data);
        }
    }
}
