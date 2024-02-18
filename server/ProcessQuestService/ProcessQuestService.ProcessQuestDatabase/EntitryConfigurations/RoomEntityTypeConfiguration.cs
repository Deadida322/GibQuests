using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcessQuestService.ProcessQuestDatabase.Models;

namespace ProcessQuestService.ProcessQuestDatabase.EntitryConfigurations
{
    public class RoomEntityConfiguration : BaseEntityTypeConfiguration<RoomEntity>
    {
        public override void Configure(EntityTypeBuilder<RoomEntity> builder)
        {
            base.Configure(builder);
            builder.ToTable("room");
        }
    }
}
