using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CommonDatabase.QuestDatabase.Models.Stages;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class VideoStageEntityConfiguration : StageEntityConfiguration<VideoStageEntity>
    {
        public override void Configure(EntityTypeBuilder<VideoStageEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("video_stage");
        }
    }
}
