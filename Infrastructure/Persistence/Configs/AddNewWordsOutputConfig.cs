using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniqueWords.Application.Models;

namespace UniqueWords.Infrastructure.Persistence.Configs
{
    public class AddNewWordsOutputConfig : IEntityTypeConfiguration<AddNewWordsOutput>
    {
        public void Configure(EntityTypeBuilder<AddNewWordsOutput> builder)
        {
            builder.HasNoKey();
        }
    }
}
