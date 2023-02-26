using CommonDatabase.QuestDatabase.Models;
using CommonDatabase.QuestDatabase.Models.Stages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CommonDatabase.QuestDatabase
{
    public class QuestContext : DbContext
    {
        public DbSet<QuestEntity> Quests { get; set; }
        public DbSet<CoordinatesEntity> Coordinates { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<StageEntity> Stages { get; set; }
        public DbSet<MapStageEntity> MapStages { get; set; }
        public DbSet<QrCodeStageEntity> QrCodeStages { get; set; }
        public DbSet<TestStageEntity> TestStages { get; set; }
        public DbSet<TextStageEntity> TextStages { get; set; }
        public DbSet<VideoStageEntity> VideoStages { get; set; }

        public QuestContext(DbContextOptions<QuestContext> options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
