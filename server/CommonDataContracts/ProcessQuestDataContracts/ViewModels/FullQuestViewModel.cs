using ProcessQuestDataContracts.Models;
using ProcessQuestDataContracts.Models.Stages;

namespace ProcessQuestDataContracts.ViewModels
{
    public class FullQuestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public QuestPolicyProcess Policy { get; set; }

        public IList<StageProcess> Stages { get; set; }

        /// <summary>
        /// ИД Автора квеста
        /// </summary>
        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
