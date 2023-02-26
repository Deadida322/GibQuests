namespace GenerateQuestsService.DataContracts.DataContracts
{
    public class ShortQuestViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public int StageCount { get; set; }
        /// <summary>
        /// ИД Автора квеста
        /// </summary>
        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
