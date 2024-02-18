using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcessQuestService.ProcessQuestDatabase.Models;

namespace ProcessQuestService.ProcessQuestDatabase.EntitryConfigurations
{
    /// <summary>
    /// Базовая сущность конфигурации
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasIndex(e => e.Id).IsUnique();
            builder.Property(p => p.IsDeleted).HasColumnName("is_deleted")
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
