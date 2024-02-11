namespace ProcessQuestDataContracts.ViewModels
{
    public class StartQuestViewModel
    {
        /// <summary>
        /// Комната открывшегося прохождения
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Токен доступа к прохождению
        /// </summary>
        public string Token { get; set; }

        public QuestProcessViewModel Quest { get; set; }
    }
}
