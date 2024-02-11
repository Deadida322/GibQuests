using CommonInfrastructure.Extension;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Interfaces;
using GenerateQuestsService.DataContracts.Models.Stages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ProcessQuestService.Core.HelperModels;
using System.Text;

namespace ProcessQuestService.Core.Helpers
{
    public class ProcessQuestCacheHelper
    {
        private IDistributedCache _cache;
        private RedisSetting _redisSetting;
        private QuestJsonSerializer _jsonSerializer;
        private IGenerateQuestsApi _generateQuestsApi;
        public ProcessQuestCacheHelper(IDistributedCache cache,
            IOptions<RedisSetting> redisSetting, QuestJsonSerializer jsonSerializer,
            IGenerateQuestsApi generateQuestsApi)
        {
            _cache = cache;
            _redisSetting = redisSetting.Value;
            _jsonSerializer = jsonSerializer;
            _generateQuestsApi = generateQuestsApi;
        }

        private ProcessModel GetProcessModel(string processString)
        {
            if (processString.IsNullOrEmpty())
            {
                return null;
            }
            return _jsonSerializer.Deserialize<ProcessModel>(processString);
        }

        private async Task<QuestViewModel> GetQuestViewModel(string questId)
        {
            if (questId.IsNullOrEmpty())
            {
                return null;
            }
            string questString = await _cache.GetStringAsync(questId);

            if (questString.IsNullOrEmpty())
            {
                return null;
            }
            return _jsonSerializer.Deserialize<QuestViewModel>(questString);
        }


        public async Task<RegisterProcessUserModel> RegisterProcessingAsync(int userId, int questId)
        {
            //мб проверить в базе - есть ли прохождение пользователя
            string roomKey = await GenerateNumberRoom();

            ProcessModel process = new()
            {
                Key = roomKey,
                QuestId = questId.ToString()
            };
            string userToken = GenerateUserToken();
            ProcessUserModel userModel = new()
            {
                Stage = 0,
                Token = userToken
            };
            process.UserProcessing.Add(userId, userModel);

            await _cache.SetStringAsync(roomKey, _jsonSerializer.Serialize(process));
            return new RegisterProcessUserModel
            {
                Room = roomKey,
                Token = userToken
            };
        }

        private async Task PassProcessingAsync(string roomKey, int userId, int stageOrder)
        {
            string processString = await _cache.GetStringAsync(roomKey);
            ProcessModel process;
            if (!processString.IsNullOrEmpty())
            {
                process = GetProcessModel(processString);
                process.UserProcessing[userId].Stage = stageOrder;
                await _cache.SetStringAsync(roomKey, _jsonSerializer.Serialize(process));
            }
        }

        private async Task RemoveProcessingAsync(string roomKey, int userId)
        {
            string processString = await _cache.GetStringAsync(roomKey);
            ProcessModel process;
            if (!processString.IsNullOrEmpty())
            {
                process = GetProcessModel(processString);
                if (process.UserProcessing.ContainsKey(userId))
                {
                    process.UserProcessing.Remove(userId);
                    if (process.UserProcessing.Count == 0)
                    {
                        await RemoveRoomAsync(roomKey);
                    }
                    else
                    {
                        await _cache.SetStringAsync(roomKey, _jsonSerializer.Serialize(process));
                    }
                }
            }
        }

        private async Task AddRoomAsync(string roomKey)
        {
            string rooms = await _cache.GetStringAsync(_redisSetting.RoomKey);
            if (rooms.IsNullOrEmpty())
            {
                await _cache.SetStringAsync(_redisSetting.RoomKey, roomKey);
            }
            else
            {
                rooms += ',' + roomKey;
                await _cache.SetStringAsync(_redisSetting.RoomKey, rooms);
            }
        }

        private async Task RemoveRoomAsync(string roomKey)
        {
            string rooms = await _cache.GetStringAsync(_redisSetting.RoomKey);
            if (!rooms.IsNullOrEmpty())
            {
                rooms = rooms.Replace(roomKey + ",", "");
                rooms = rooms.Replace(roomKey, "");
                await _cache.SetStringAsync(_redisSetting.RoomKey, rooms);
            }
        }

        public async Task<bool> IsSetRoomKeyAsync(string roomKey)
        {
            //если не пришел ключ - значит такая комната есть, условно
            if (!string.IsNullOrEmpty(roomKey))
            {
                //если есть ключи комнат и такой уже есть - возвращаем тру
                string rooms = await _cache.GetStringAsync(_redisSetting.RoomKey);

                if (!rooms.IsNullOrEmpty() && rooms.Contains(roomKey))
                {
                    return false;
                }
                await AddRoomAsync(roomKey);
                return true;
            }
            return false;
        }

        public async Task<IList<ProcessModel>> GetQuestProcessModelsAsync(string questId)
        {
            var result = new List<ProcessModel>();

            string roomsStr = await _cache.GetStringAsync(_redisSetting.RoomKey);
            if (roomsStr.IsNullOrEmpty())
            {
                return result;
            }
            foreach (var roomKey in roomsStr.Split(','))
            {
                string roomString = await _cache.GetStringAsync(roomKey);
                var processModel = roomString.IsNullOrEmpty() ? null : GetProcessModel(roomString);
                if (processModel != null && processModel.QuestId == questId)
                {
                    result.Add(processModel);
                }
            }
            return result;
        }

        public async Task<ProcessModel> GetUserQuestProcessModelAsync(string questId, int userId)
        {
            var processModels = new List<ProcessModel>();

            string roomsStr = await _cache.GetStringAsync(_redisSetting.RoomKey);
            if (roomsStr.IsNullOrEmpty())
            {
                return null;
            }
            foreach (var roomKey in roomsStr.Split(','))
            {
                string roomString = await _cache.GetStringAsync(roomKey);
                var processModel = roomString.IsNullOrEmpty() ? null : GetProcessModel(roomString);
                if (processModel != null && processModel.QuestId == questId)
                {
                    processModels.Add(processModel);
                }
            }
            foreach (var process in processModels)
            {
                if (process.UserProcessing.ContainsKey(userId))
                {
                    return process;
                }
            }
            return null;
        }

        public async Task<List<ProcessModel>> GetAllUserQuestProcessModelsAsync(int userId)
        {
            var processModels = new List<ProcessModel>();

            string roomsStr = await _cache.GetStringAsync(_redisSetting.RoomKey);
            if (roomsStr.IsNullOrEmpty())
            {
                return null;
            }
            foreach (var roomKey in roomsStr.Split(','))
            {
                string roomString = await _cache.GetStringAsync(roomKey);
                var processModel = roomString.IsNullOrEmpty() ? null : GetProcessModel(roomString);
                if (processModel != null && processModel.UserProcessing.ContainsKey(userId))
                {
                    processModels.Add(processModel);
                }
            }
            return processModels;
        }

        public async Task<bool> SetQuestAsync(QuestViewModel quest)
        {
            quest.Stages = quest.Stages.OrderBy(q => q.Order).ToList();

            string questId = quest != null ? quest.Id.ToString() : null;
            if (questId == null)
            {
                return false;
            }
            else if (await _cache.GetStringAsync(questId) != null)
            {
                //делаем для того, чтобы обновилось время хранения квеста
                _cache.RefreshAsync(_cache.GetString(questId));
                return true;
            }
            else
            {
                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                //кешируем в зависимости от длительности квеста  ?
                await _cache.SetStringAsync(questId,
                    _jsonSerializer.Serialize(quest),
                    cacheOptions
                   );
                return true;
            }
        }

        /// <summary>
        /// Получение квеста по ключу комнаты
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public async Task<QuestViewModel> GetQuestAsync(string roomKey)
        {
            string roomString = await _cache.GetStringAsync(roomKey);
            if (roomKey.IsNullOrEmpty())
            {
                return null;
            }
            var process = GetProcessModel(roomString);
            return process == null ? null :
                await GetQuestViewModel(process.QuestId);
        }

        /// <summary>
        /// Получение квеста по id Квеста
        /// </summary>
        public async Task<QuestViewModel> GetQuestForIdAsync(string questId)
        {
            return await GetQuestViewModel(questId);
        }

        public async Task<Stage> GetNextStageAsync(string questId, Stage currentStage, string room, int userId)
        {
            var quest = await GetQuestViewModel(questId);
            if (quest == null) return null;
            if (currentStage.Order == quest.Stages.Count - 1)
            {
                await RemoveProcessingAsync(room, userId);
                return new FinalStage();
            }
            else
            {
                int nextStage = currentStage.Order + 1;
                await PassProcessingAsync(room, userId, nextStage);
                return quest.Stages.Where(el => el.Order == nextStage).FirstOrDefault();
            }
        }

        //новые функции
        public async Task<QuestViewModel?> GetAsync(int questId)
        {
            string questString = await _cache.GetStringAsync(questId.ToString());

            if (questString.IsNullOrEmpty())
            {
                var questRes = await _generateQuestsApi.GetQuestAsync(
                    new GetQuestContract
                    {
                        Id = questId
                    }
                );
                if (!questRes.Success || questRes.Data == null)
                {
                    return null;
                }
                var quest = questRes.Data;

                quest.Stages = quest.Stages.OrderBy(q => q.Order).ToList();

                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                //кешируем в зависимости от длительности квеста  ?
                await _cache.SetStringAsync(questId.ToString(),
                    _jsonSerializer.Serialize(quest),
                    cacheOptions
                    );

                return quest;
            }
            else
            {
                return _jsonSerializer.Deserialize<QuestViewModel>(questString);
            }
        }

        private async Task<string> GenerateNumberRoom()
        {
            int attempt = 0;
            while (attempt < 10)
            {
                string value = Guid.NewGuid().ToString();
                var room = await _cache.GetStringAsync(value);
                if (room.IsNullOrEmpty())
                {
                    return value;
                }
                attempt++;
            }
            return "";
        }

        private static string GenerateUserToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
