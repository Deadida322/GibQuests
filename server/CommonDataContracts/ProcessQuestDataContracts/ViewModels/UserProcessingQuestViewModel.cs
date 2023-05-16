using ProcessQuestDataContracts.Models.Stages;

namespace ProcessQuestDataContracts.ViewModels
{
    public class UserProcessingQuestViewModel
    {
        public int QuestId { get; set; }

        public string QuestName { get; set; }

        public string Room { get; set; }

        public StageProcess Stage { get; set; }
    }
}
