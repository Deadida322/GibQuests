using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CommonDatabase.QuestDatabase.Models.Stages;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class TestStageEntityConfiguration : StageEntityConfiguration<TestStageEntity>
    {
        public override void Configure(EntityTypeBuilder<TestStageEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("test_stage");
        }
    }
}
