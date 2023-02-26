using CommonDatabase.QuestDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class QuestPolicyEntityConfiguration : BaseEntityTypeConfiguration<QuestPolicyEntity>
    {
        public override void Configure(EntityTypeBuilder<QuestPolicyEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("quest_policy");
        }
    }
}
