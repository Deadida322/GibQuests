using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CommonDatabase.QuestDatabase.Models.Stages;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class QrCodeStageEntityConfiguration : StageEntityConfiguration<QrCodeStageEntity>
    {
        public override void Configure(EntityTypeBuilder<QrCodeStageEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("qrcode_stage");
        }
    }
}
