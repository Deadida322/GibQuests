using GenerateQuestsService.DataContracts.DataContracts;

namespace CommonDatabase.QuestDatabase.Interfaces
{
    public interface IGenerateQuestStorage
    {
        public Task CreateQuestAsync(CreateQuestContract contract);

        public Task<bool> UpdateQuestAsync(UpdateQuestContract contract);

        public Task<QuestViewModel> GetQuestAsync(GetQuestContract contract);

        public Task<IList<ShortQuestViewModel>> GetFilteredQuestsAsync(GetFilteredQuestsContract contract);

        public Task<bool> DeleteQuestAsync(DeleteQuestContract contract);
    }
}
