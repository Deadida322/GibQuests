using Microsoft.EntityFrameworkCore;
using ProcessQuestService.ProcessQuestDatabase.Models;

namespace ProcessQuestService.ProcessQuestDatabase
{
    public class ProcessQuestContext : DbContext
    {
        public DbSet<RoomEntity> Rooms { get; set; }

        public DbSet<PassingUserEntity> PassedUsers { get; set; }
    }
}
