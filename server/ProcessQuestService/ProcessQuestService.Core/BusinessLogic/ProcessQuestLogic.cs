using AutoMapper;
using GenerateQuestsService.DataContracts.Enums;
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
        private ProcessQuestCacheHelper _cacheHelper;
        private QuestJsonSerializer _jsonSerializer;
        private IMapper _mapper;

        public ProcessQuestLogic(ProcessQuestCacheHelper cacheHelper, IMapper mapper, QuestJsonSerializer jsonSerializer) { 
            _cacheHelper = cacheHelper;
            _mapper = mapper;
            _jsonSerializer = jsonSerializer;
        }

        private WebSocketRequest Deserialize(byte[] buffer, int length)
        {
            byte[] data = new byte[length];
            Array.Copy(buffer, data, length);
            string requestString = Encoding.UTF8.GetString(data);
            return _jsonSerializer.Deserialize<WebSocketRequest>(requestString);
        }                                       

        public byte[] Serialize(WebSocketResponse response)
        {
            return _jsonSerializer.SerializeAsByte(response);

        }

        public async Task<WebSocketResponse> ProcessAsync(byte[] buffer, int length, string room)
        {
            try
            {
                var request = Deserialize(buffer, length);
                if (request == null)
                {
                    return BuildWebSocketResponseHelper.BuildErrorModelResponse();
                }
                var quest = await _cacheHelper.GetQuestAsync(room);
                if(quest == null)
                {
                    return BuildWebSocketResponseHelper.BuildErrorResponse("Нет такого квеста");
                }
                var questStage = quest.Stages.Where(el => el.Equals(request.Stage)).FirstOrDefault();

                if(questStage == null)
                {
                    return BuildWebSocketResponseHelper.BuildErrorResponse("Нет такого этапа");
                }
                if(StageProcessing(request.Stage, questStage))
                {
                    var nextStage = await _cacheHelper.GetNextStageAsync(quest.Id.ToString(), questStage,room, request.UserId);
                    if(nextStage == null)
                    {
                        return BuildWebSocketResponseHelper.BuildErrorResponse("Нет следующего этапа");
                    }
                    return BuildWebSocketResponseHelper.BuildSuccessResponse(
                        _mapper.Map<StageProcess>(nextStage));
                }     
                return BuildWebSocketResponseHelper.BuildErrorChoiseResponse();

            }
            catch (Exception ex)
            {
                return BuildWebSocketResponseHelper.BuildErrorResponse(ex.Message);
            }
        }

        public async Task<WebSocketResponse> ProcessAsync(string room, WebSocketRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BuildWebSocketResponseHelper.BuildErrorModelResponse();
                }
                var quest = await _cacheHelper.GetQuestAsync(room);
                if (quest == null)
                {
                    return BuildWebSocketResponseHelper.BuildErrorResponse("Нет такого квеста");
                }
                var questStage = quest.Stages.Where(el => el.Equals(request.Stage)).FirstOrDefault();

                if (questStage == null)
                {
                    return BuildWebSocketResponseHelper.BuildErrorResponse("Нет такого этапа");
                }
                if (StageProcessing(request.Stage, questStage))
                {
                    var nextStage = await _cacheHelper.GetNextStageAsync(quest.Id.ToString(), questStage, room, request.UserId);
                    if (nextStage == null)
                    {
                        return BuildWebSocketResponseHelper.BuildErrorResponse("Нет следующего этапа");
                    }
                    return BuildWebSocketResponseHelper.BuildSuccessResponse(
                        _mapper.Map<StageProcess>(nextStage));
                }
                return BuildWebSocketResponseHelper.BuildErrorChoiseResponse();

            }
            catch (Exception ex)
            {
                return BuildWebSocketResponseHelper.BuildErrorResponse(ex.Message);
            }
        }
        private bool StageProcessing<T>(T userStage, T questStage) where T : Stage
        {
            switch(userStage.Type)
            {
                case StageType.Map:
                    return (userStage is MapStage && questStage is MapStage) ?
                        IsReadyMapStage(userStage as MapStage, questStage as MapStage) : false;
                case StageType.QrCode:
                    return (userStage is QrCodeStage && questStage is QrCodeStage) ?
                        IsReadyQrCodeStage(userStage as QrCodeStage, questStage as QrCodeStage) : false;
                case StageType.Test:
                    return (userStage is TestStage && questStage is TestStage) ?
                        IsReadyTestStage(userStage as TestStage, questStage as TestStage) : false;
                case StageType.Text:
                    return (userStage is TextStage && questStage is TextStage) ?
                        IsReadyTextStage(userStage as TextStage, questStage as TextStage) : false;
                case StageType.Video:
                    return (userStage is VideoStage && questStage is VideoStage) ?
                        IsReadyVideoStage(userStage as VideoStage, questStage as VideoStage) : false;
                default: return false;

            }

        }

        /// <summary>
        /// Разность квадратов ширины и долготы
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userStage"></param>
        /// <param name="questStage"></param>
        /// <returns></returns>
        private bool IsReadyMapStage(MapStage userStage, MapStage questStage)
        {
            //Должна задаваться точность в этапе квеста
            double distance = (Math.Pow((double)(userStage.Coords.Longitude - questStage.Coords.Longitude), 2) +
                Math.Pow((double)(userStage.Coords.Latitude - questStage.Coords.Latitude), 2));
            return distance < 10;
        }

        private bool IsReadyQrCodeStage(QrCodeStage userStage, QrCodeStage questStage)
        {
            return userStage.Code == questStage.Code;
        }

        private bool IsReadyTextStage(TextStage userStage, TextStage questStage)
        {
            return userStage.Text == questStage.Text;
        }

        private bool IsReadyVideoStage(VideoStage userStage, VideoStage questStage)
        {
            return userStage.Url == questStage.Url;
        }

        private bool IsReadyTestStage(TestStage userStage, TestStage questStage)
        {
            //сортируем по порядку
            var userQuestions = userStage.Questions.OrderBy(el => el.Order).ToArray();
            var rightQuestions = questStage.Questions.OrderBy(el => el.Order).ToArray();
            
            if(userQuestions.Length != rightQuestions.Length) return false;
            
            for(int i = 0; i < rightQuestions.Count(); i++)
            {
                 //проверка на то, что есть правильные ответы
                if(userQuestions[i].RightAnswers == null || userQuestions[i].RightAnswers.Length == 0 ||
                    rightQuestions[i].RightAnswers == null || rightQuestions[i].RightAnswers.Length == 0)
                {
                    return false;
                }
                //устанавливаем нижний регистр
                var questAnswers = userQuestions[i].RightAnswers.Select(el => el.ToLower()).ToArray();
                var rightAnswers = rightQuestions[i].RightAnswers.Select(el => el.ToLower()).ToArray();

                if(!IsEqualsArray(questAnswers, rightAnswers))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsEqualsArray(string[] a, string[] b)
        {
            if(a.Length != b.Length) return false;
            for(int i = 0; i < a.Length; i++)
            {
                if (a[i]!= b[i]) return false;
            }
            return true;
        }
        
    }
}
