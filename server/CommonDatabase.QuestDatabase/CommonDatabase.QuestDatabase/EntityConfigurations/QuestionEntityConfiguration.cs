using CommonDatabase.QuestDatabase.Models.Stages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommonDatabase.QuestDatabase.EntityConfigurations
{
    public class QuestionEntityConfiguration : BaseEntityTypeConfiguration<QuestionEntity>
    {
        public override void Configure(EntityTypeBuilder<QuestionEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("question");
        }
    }
}
