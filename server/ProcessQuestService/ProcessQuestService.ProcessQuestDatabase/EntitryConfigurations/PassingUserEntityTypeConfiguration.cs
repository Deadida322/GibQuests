using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessQuestService.ProcessQuestDatabase.Models;

namespace ProcessQuestService.ProcessQuestDatabase.EntitryConfigurations
{
    public class PassingUserEntityTypeConfiguration : BaseEntityTypeConfiguration<PassingUserEntity>
    {
        public override void Configure(EntityTypeBuilder<PassingUserEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("passing_user");
        }
    }
}
