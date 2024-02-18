namespace ProcessQuestService.ProcessQuestDatabase.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
