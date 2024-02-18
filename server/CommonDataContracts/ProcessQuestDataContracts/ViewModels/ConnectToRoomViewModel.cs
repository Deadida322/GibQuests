namespace ProcessQuestDataContracts.ViewModels
{
    public class ConnectToRoomViewModel
    {
        /// <summary>
        /// Комната открывшегося прохождения
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// Токен доступа к прохождению
        /// </summary>
        public string Token { get; set; }

        public QuestProcessViewModel Quest { get; set; }
    }
}
