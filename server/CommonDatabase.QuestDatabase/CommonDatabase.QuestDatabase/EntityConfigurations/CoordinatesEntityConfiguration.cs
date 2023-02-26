using CommonDatabase.QuestDatabase.Models.Stages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class CoordinatesEntityConfiguration : BaseEntityTypeConfiguration<CoordinatesEntity>
    {
        public override void Configure(EntityTypeBuilder<CoordinatesEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("coordinates");
        }
    }
}
