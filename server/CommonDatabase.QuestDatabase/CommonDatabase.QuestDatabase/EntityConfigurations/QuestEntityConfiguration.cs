using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CommonDatabase.QuestDatabase.Models;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class QuestEntityConfiguration : BaseEntityTypeConfiguration<QuestEntity>
    {
        public override void Configure(EntityTypeBuilder<QuestEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("quest");
        }
    }
}
