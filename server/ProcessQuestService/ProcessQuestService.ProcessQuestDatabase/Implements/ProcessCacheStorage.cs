using AutoMapper;
using CommonInfrastructure.Extension;
using Microsoft.Extensions.Caching.Distributed;
using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.ProcessModels;
using ProcessQuestDataContracts.ViewModels;
using ProcessQuestService.ProcessQuestDatabase.Helpers;
using ProcessQuestService.ProcessQuestDatabase.Interfaces;
using System.Text;

namespace ProcessQuestService.ProcessQuestDatabase.Implements
{
    public class ProcessCacheStorage : IProcessCacheStorage
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

        public async Task<FullQuestViewModel?> GetQuestAsync(GetProcessQuestContract contract)
        {
            string? questString = await _cache.GetStringAsync(contract.Id.ToString());

            if (!questString.IsNullOrEmpty())
            {
                return _jsonSerializer.Deserialize<FullQuestViewModel>(questString);

            }
            return null;
        }

        public async Task<bool> IsRoomExist(Guid roomKey)
        {
            var room = await _cache.GetStringAsync(roomKey.ToString());
            return !room.IsNullOrEmpty();
        }

        public async Task<bool> SetQuestAsync(FullQuestViewModel quest)
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

        public async Task<ProcessModel?> GetRoomAsync(CommonRoomContract contract)
        {
            string? processString = await _cache.GetStringAsync(contract.Key.ToString());
            if (!processString.IsNullOrEmpty())
            {
                return _jsonSerializer.Deserialize<ProcessModel>(processString);

            }
            return null;
        }

        public async Task<ConnectToRoomViewModel?> ConnectToRoomAsync(
            CommonRoomContract contract, 
            UserDataViewModel user, 
            string userToken
            )
        {

            var processRoom = await GetRoomAsync(contract);
            if(processRoom == null)
            {
                return null;
            }

            ProcessUserModel userModel = new()
            {
                Stage = 0,
                Token = userToken,
                UserData = user
            };
            if (processRoom.UserProcessing.Values.Select(el => el.Token).Contains(userModel.Token)) {
                return null;                                            
            }
            processRoom.UserProcessing.Add(user.Id, userModel);

            await UpdateRoomAsync(processRoom);

            return new ConnectToRoomViewModel
            {
                Key = processRoom.Key,
                Token = userToken
            };
        }

        public async Task UpdateRoomAsync(ProcessModel room)
        {
            await _cache.SetStringAsync(room.Key.ToString(), _jsonSerializer.Serialize(room));
        }
    }
}
