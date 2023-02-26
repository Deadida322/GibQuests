using ProcessQuestDataContracts.Models.Stages;

namespace ProcessQuestService.Core.InteractionWebSocketModel
{
    public class WebSocketResponse
    {
        /// <summary>
        /// Поле успешности, если false - 
        /// то внутренняя ошибка, не связанная с прохождением
        /// </summary>
        public bool Sucsess { get; set; }

        /// <summary>
        /// В случае успеха возвращается новый этап
        /// </summary>
        public StageProcess Stage { get; set; }

        /// <summary>
        /// Ошибка, внутренняя или внешняя
        /// </summary>
        public string Error { get; set; }
    }
}
