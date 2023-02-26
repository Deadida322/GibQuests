using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CommonDatabase.QuestDatabase.Models.Stages;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class MapStageEntityConfiguration : StageEntityConfiguration<MapStageEntity>
    {
        public override void Configure(EntityTypeBuilder<MapStageEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("map_stage");
        }
    }
}
