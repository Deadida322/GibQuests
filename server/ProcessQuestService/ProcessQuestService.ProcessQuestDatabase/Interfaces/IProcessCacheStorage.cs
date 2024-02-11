using GenerateQuestsService.DataContracts.DataContracts;
using ProcessQuestDataContracts.DataContracts;

namespace ProcessQuestService.ProcessQuestDatabase.Interfaces
{
    public interface IProcessCacheStorage
    {
        public Task<QuestViewModel?> GetQuestAsync(GetProcessQuestContract contract);

        public Task<bool> SetQuestAsync(QuestViewModel quest);

        public Task<bool> IsRoomExist(Guid roomKey);

        public Task CreateRoomAsync(RegisterRoomContract contract);
    }
}
