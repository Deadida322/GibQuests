using CommonInfrastructure.Extension;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.JsonHelpers;
using GenerateQuestsService.DataContracts.Models.Stages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using ProcessQuestDataContracts.Models.Stages;
using ProcessQuestService.Core.HelperModels;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace ProcessQuestService.Core.Helpers
{
    public class ProcessQuestCacheHelper
    {
        private IDistributedCache _cache;
        private RedisSetting _redisSetting;
        private QuestJsonSerializer _jsonSerializer;
        public ProcessQuestCacheHelper(IDistributedCache cache,
            IOptions<RedisSetting> redisSetting, QuestJsonSerializer jsonSerializer)
        {
            _cache = cache;
            _redisSetting = redisSetting.Value;
            _jsonSerializer = jsonSerializer;
        }

        private ProcessModel GetProcessModel(string processString)
        {
            if(processString.IsNullOrEmpty())
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

            if(questString.IsNullOrEmpty())
            {
                return null;
            }
            return _jsonSerializer.Deserialize<QuestViewModel>(questString);
        }
        public async Task RegisterProcessingAsync(string roomKey, int userId, string questId)
        {
            string processString =  await _cache.GetStringAsync(roomKey);
            ProcessModel process;
            if(processString.IsNullOrEmpty()) {
                process = new ProcessModel()
                {
                    Key = roomKey,
                    QuestId = questId
                };
                process.UserProcessing.Add(userId, 0);
            }
            else
            {
                process = GetProcessModel(processString);
                process.UserProcessing.Add(userId, 0);
            }
            await _cache.SetStringAsync(roomKey, _jsonSerializer.Serialize(process));

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

        public async Task<bool> IsSetRoomKeyAsync(string roomKey)
        {
            //если не пришел ключ - значит такая комната есть, условно
            if (!string.IsNullOrEmpty(roomKey))
            {
                //если есть ключи комнат и такой уже есть - возвращаем тру
                string rooms = await _cache.GetStringAsync(_redisSetting.RoomKey);

                if(!rooms.IsNullOrEmpty() && rooms.Contains(roomKey)) {
                    return false;
                }
                await AddRoomAsync(roomKey);
                return true;
            }
            return false;
        }

        public async Task<bool> SetQuestAsync(QuestViewModel quest)
        {
            string questId = quest != null ? quest.Id.ToString() : null;
            if (questId == null)
            {
                return false;
            }
            else if(await _cache.GetStringAsync(questId) != null)
            {
                //делаем для того, чтобы обновилось время хранения квеста
                _cache.RefreshAsync(_cache.GetString(questId));
                return true;
            }
            else
            {
                //кешируем в зависимости от длительности квеста
                await _cache.SetStringAsync(questId,
                    _jsonSerializer.Serialize(quest),
                    new DistributedCacheEntryOptions()
                {
                      AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
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
            if(roomKey.IsNullOrEmpty())
            {
                return null;
            }
            var process = GetProcessModel(roomString);
            return process == null ? null : 
                await GetQuestViewModel(process.QuestId);
        }

        public async Task<Stage> GetNextStageAsync(string questId, Stage currentStage)
        {
            var quest = await GetQuestViewModel(questId);
            if (quest == null) return null;
            if(currentStage.Order == quest.Stages.Count - 1)
            {
                return new FinalStage();
            }
            return quest.Stages.Where(el => el.Order == currentStage.Order + 1).FirstOrDefault();
        }

        public bool IsQuestReady(int id)
        {
            return false;
        }
    }
}
