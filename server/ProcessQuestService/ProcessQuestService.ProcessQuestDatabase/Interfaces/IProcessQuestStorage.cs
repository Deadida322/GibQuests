using ProcessQuestDataContracts.DataContracts;

namespace ProcessQuestService.ProcessQuestDatabase.Interfaces
{
    public interface IProcessQuestStorage
    {
        public Task<bool> IsRoomExist(Guid roomKey);

        public Task CreateRoomAsync(RegisterRoomContract contract);
    }
}
