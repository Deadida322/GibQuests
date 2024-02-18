using Microsoft.EntityFrameworkCore;
using ProcessQuestService.ProcessQuestDatabase.Models;
using System.Reflection;

namespace ProcessQuestService.ProcessQuestDatabase
{
    public class ProcessQuestContext : DbContext
    {
        public DbSet<RoomEntity> Rooms { get; set; }

        public DbSet<PassingUserEntity> PassedUsers { get; set; }

        public ProcessQuestContext(DbContextOptions<ProcessQuestContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
