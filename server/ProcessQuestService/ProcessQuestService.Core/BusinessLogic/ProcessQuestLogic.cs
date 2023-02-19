using GenerateQuestsService.DataContracts.JsonHelpers;
using GenerateQuestsService.DataContracts.Models.Stages;
using ProcessQuestDataContracts.Models.Stages;
using ProcessQuestService.Core.Helpers;
using ProcessQuestService.Core.InteractionWebSocketModel;
using System.Text;
using System.Text.Json;

namespace ProcessQuestService.Core.BusinessLogic
{
    public class ProcessQuestLogic
    {
        private JsonSerializerOptions _jsonSerializerOptions;
        private ProcessQuestCacheHelper _cacheHelper;

        public ProcessQuestLogic(ProcessQuestCacheHelper cacheHelper) { 
            _jsonSerializerOptions = new JsonSerializerOptions().SetQuestJsonSerializerOptions();
            _cacheHelper = cacheHelper;
        }

        private WebSocketRequest Deserialize(byte[] buffer, int length)
        {
            string requestString = Encoding.UTF8.GetString(buffer).Substring(0, length);
            return JsonSerializer.Deserialize<WebSocketRequest>(requestString, _jsonSerializerOptions);
        }

        public async Task<WebSocketResponse> ProcessAsync(byte[] buffer, int length, string room)
        {
            try
            {
                var request = Deserialize(buffer, length);
                if (request == null)
                {
                    BuildWebSocketResponseHelper.BuildErrorModelResponse();
                }
                var quest = await _cacheHelper.GetQuest(room);
                if(quest == null)
                {
                    BuildWebSocketResponseHelper.BuildErrorResponse("Нет такого квеста");
                }
                var questStage = quest.Stages.Where(el => el.Equals(request.Stage)).FirstOrDefault();

                if(questStage == null)
                {
                    BuildWebSocketResponseHelper.BuildErrorResponse("Нет такого этапа");
                }
                if(StageProcessing(request.Stage, questStage))
                {
                    return
                }
                return BuildWebSocketResponseHelper.BuildErrorChoiseResponse();

            }
            catch (Exception ex)
            {
                BuildWebSocketResponseHelper.BuildErrorResponse(ex.Message);
            }
        }
        private bool StageProcessing(Stage userStage, Stage questStage)
        {
            switch(userStage.GetType())
            {
                case Type.GetType(MapStage):
                    return
            }

        }
    }
}
