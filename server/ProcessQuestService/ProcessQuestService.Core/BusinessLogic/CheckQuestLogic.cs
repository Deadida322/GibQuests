using AuthService.DataContracts.Interfaces;
using AuthService.DataContracts.User;
using AutoMapper;
using ProcessQuestService.Core.HelperModels;
using ProcessQuestService.Core.Helpers;
using ProcessQuestService.Core.InteractionWebSocketModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessQuestService.Core.BusinessLogic
{
    public class CheckQuestLogic
    {
        private ProcessQuestCacheHelper _cacheHelper;
        private QuestJsonSerializer _jsonSerializer;
        IAuthApi _authApi;
        private IMapper _mapper;
        public CheckQuestLogic(ProcessQuestCacheHelper cacheHelper, IMapper mapper, QuestJsonSerializer jsonSerializer, IAuthApi authApi)
        {
            _cacheHelper = cacheHelper;
            _mapper = mapper;
            _jsonSerializer = jsonSerializer;
            _authApi = authApi;
        }

        public async Task<byte[]> GetQuestProcessingSerializeAsync(string questId) {
            var result = new List<CheckQuestModel>();

            //получаеи все прохождения по этому квесту
            var processModels = await _cacheHelper.GetQuestProcessModelsAsync(questId);
            
            //получаем инфу по квесту
            var quest = await _cacheHelper.GetQuestForIdAsync(questId);

            if (quest == null || processModels == null || processModels.Count == 0) {
                return _jsonSerializer.SerializeAsByte(result);
            }
            //результат: пользователь + прохождение в квесте

            //продимся по моделям прохождения каждой комнаты
            foreach(var process in processModels)
            {
                //смотрим на список юзеров и их этапов (ключ - значение)
                foreach(var userProcess in process.UserProcessing)
                {
                    //получаем пользователя
                    var usserRes = await _authApi.GetUserByIdAsync(userProcess.Key);
                    var userInfo = new ShortUserViewModel();
                    if (usserRes.Success)
                    {
                        userInfo = usserRes.Data;
                    }
                    var userStage = quest.Stages[userProcess.Value.Stage];
                    var userProgress = new ProgressUserModel()
                    {
                        Stage = userStage.Order,
                        StageName = userStage.Title
                    };
                    result.Add(new CheckQuestModel
                    {
                        Progress = userProgress,
                        User = userInfo
                    });
                }
            }
            return _jsonSerializer.SerializeAsByte(result);
        }
    }
}
