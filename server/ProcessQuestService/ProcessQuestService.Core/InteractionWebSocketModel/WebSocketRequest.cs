using GenerateQuestsService.DataContracts.Models.Stages;

namespace ProcessQuestService.Core.InteractionWebSocketModel
{
    public class WebSocketRequest
    {
        public int UserId { get; set; }

        public Stage Stage { get; set; }

    }
}
