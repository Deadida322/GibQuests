using ProcessQuestDataContracts.DataContracts;
using ProcessQuestDataContracts.ProcessModels;
using ProcessQuestDataContracts.ViewModels;

namespace ProcessQuestService.ProcessQuestDatabase.Interfaces
{
    public interface IProcessCacheStorage
    {
        public Task<FullQuestViewModel?> GetQuestAsync(GetProcessQuestContract contract);

        public Task<bool> SetQuestAsync(FullQuestViewModel quest);

        public Task<bool> IsRoomExist(Guid roomKey);

        public Task CreateRoomAsync(RegisterRoomContract contract);

        public Task<ProcessModel> GetRoomAsync(CommonRoomContract contract);

        public Task<ConnectToRoomViewModel?> ConnectToRoomAsync(CommonRoomContract contract, UserDataViewModel user, string userToken);

        public Task UpdateRoomAsync(ProcessModel room);
    }
}
