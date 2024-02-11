using ProcessQuestDataContracts.Enums;

namespace ProcessQuestService.ProcessQuestDatabase.Models
{
    public class RoomEntity : BaseEntity
    {
        public Guid Key { get; set; }

        public int QuestId { get; set; }

        public IList<PassingUserEntity> PassedUsers { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MaxDuration { get; set; }

        public byte Status { get; set; } = (byte)RoomStatusType.Create;
    }
}
