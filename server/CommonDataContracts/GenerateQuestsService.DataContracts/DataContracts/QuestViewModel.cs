using GenerateQuestsService.DataContracts.Models.Stages;

namespace GenerateQuestsService.DataContracts.DataContracts
{
    public class QuestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public IList<Stage> Stages { get; set; }

        /// <summary>
        /// ИД Автора квеста
        /// </summary>
        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
