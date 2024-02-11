using AutoMapper;
using CommonInfrastructure.Extension;
using GenerateQuestsService.DataContracts.DataContracts;
using GenerateQuestsService.DataContracts.Models;
using Microsoft.Extensions.Caching.Distributed;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.ProcessModels;
using ProcessQuestService.ProcessQuestDatabase.Helpers;
using ProcessQuestService.ProcessQuestDatabase.Interfaces;
using System.Diagnostics;
using System.Text;

namespace ProcessQuestService.ProcessQuestDatabase.Implements
{
    public class ProcessCacheStorage : IProcessQuestStorage
    {
        private IDistributedCache _cache;
        private QuestJsonSerializer _jsonSerializer;
        private IMapper _mapper;
        public ProcessCacheStorage(IDistributedCache cache, QuestJsonSerializer jsonSerializer, IMapper mapper)
        {
            _cache = cache;
            _jsonSerializer = jsonSerializer;
            _mapper = mapper;
        }

        public async Task<QuestViewModel?> GetQuestAsync(GetProcessQuestContract contract)
        {
            string? questString = await _cache.GetStringAsync(contract.Id.ToString());

            if (questString.IsNullOrEmpty())
            {
                return _jsonSerializer.Deserialize<QuestViewModel>(questString);

            }
            return null;
        }

        public async Task<bool> IsRoomExist(Guid roomKey)
        {
            var room = await _cache.GetStringAsync(roomKey.ToString());
            return !room.IsNullOrEmpty();
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

        public async Task CreateRoomAsync(RegisterRoomContract contract)
        {

            var process = _mapper.Map<ProcessModel>(contract);
            await _cache.SetStringAsync(process.Key.ToString(), _jsonSerializer.Serialize(process));
        }
    }
}
